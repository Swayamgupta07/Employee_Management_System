using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Auth;
using EmployeeManagementAPI.Repositories.Interfaces;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public AuthController(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IMapper mapper,
            ILogger<AuthController> logger,
            IEmailSender emailSender,
            IConfiguration configuration,
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseFactory.Fail("Invalid input", ModelState));

                var user = await _userRepository.GetUserByUsernameOrEmailAsync(loginDto.Username, cancellationToken);
                
                if (user == null)
                {
                    return Unauthorized(ApiResponseFactory.Fail("Username & Email is incorrect"));
                }

                if (user.LockoutEnd > DateTime.UtcNow)
                {
                    var timeRemaining = (user.LockoutEnd.Value - DateTime.UtcNow).Minutes + 1;
                    return StatusCode(403, ApiResponseFactory.Fail($"Account is locked due to multiple failed attempts. Try again in {timeRemaining} minutes."));
                }

                if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    user.FailedLoginAttempts++;
                    if (user.FailedLoginAttempts >= 5)
                    {
                        user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                        _logger.LogWarning("Account {Username} locked for 15 minutes", user.Username);
                    }
                    await _userRepository.UpdateAsync(user, cancellationToken);
                    
                    return Unauthorized(ApiResponseFactory.Fail("Password is incorrect"));
                }

                if (!user.IsEmailVerified)
                {
                    // Generate a new OTP since they are trying to log in but aren't verified yet
                    var newOtp = new Random().Next(100000, 999999).ToString();
                    user.OtpCode = newOtp;
                    user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
                    await _userRepository.UpdateAsync(user, cancellationToken);

                    // Send the new OTP Email with BCC to admin for testing
                    try 
                    {
                        await _emailSender.SendEmailAsync(user.Email, "Account Verification Code (Resend)", 
                            $"You attempted to login but your email is not verified.\n\nYour 6-digit verification code is: {newOtp}.\n\nThis code will expire in 5 minutes.", "swayam.gupta09@gmail.com");
                    }
                    catch (Exception emailEx) 
                    {
                        _logger.LogWarning(emailEx, "Failed to send OTP resend email during login.");
                    }

                    return StatusCode(403, ApiResponseFactory.Fail("Please verify your email. A new OTP has been sent to your registered email address."));
                }

                if (!user.IsActive)
                {
                    return StatusCode(403, ApiResponseFactory.Fail("Account pending Admin approval. You cannot login yet."));
                }

                // Reset failed attempts on success
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                await _userRepository.UpdateAsync(user, cancellationToken);

                var token = _jwtTokenService.GenerateToken(user);
                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    Expires = DateTime.UtcNow.AddHours(8)
                };

                // Send a notification email to the admin/overseer
                try 
                {
                    await _emailSender.SendEmailAsync("swayamgupta09@gmail.com", 
                        "Security Alert: New Node Access", 
                        $"System access granted.\n\nUser: {user.Username}\nEmail: {user.Email}\nRole: {user.Role}\nTimestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
                }
                catch (Exception emailEx) 
                {
                    _logger.LogWarning(emailEx, "Failed to send login notification email.");
                }

                return Ok(ApiResponseFactory.Success(response, "Login successful"));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Login operation cancelled for {Username}", loginDto.Username);
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Username}", loginDto.Username);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseFactory.Fail("Invalid input", ModelState));

                if (await _userRepository.UsernameExistsAsync(registerDto.Username, cancellationToken))
                    return Conflict(ApiResponseFactory.Fail("Username already exists"));

                if (await _userRepository.EmailExistsAsync(registerDto.Email, cancellationToken))
                    return Conflict(ApiResponseFactory.Fail("Email already exists"));

                var user = _mapper.Map<EmployeeManagementAPI.Models.Data.User>(registerDto);
                
                // Advanced Admin Workflow Logic
                if (registerDto.RequestedRole == "Admin")
                {
                    user.Role = "Employee"; // They are just an employee until approved
                    user.RequestedRole = "Admin";
                    user.IsActive = false; // Block login until approved
                }
                else
                {
                    user.Role = "Employee"; 
                    user.RequestedRole = null;
                    user.IsActive = true; 
                }
                
                // --- OTP Generation ---
                var otp = new Random().Next(100000, 999999).ToString();
                user.OtpCode = otp;
                user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
                user.IsEmailVerified = false;
                user.IsActive = false; // Must verify OTP even if employee
                // ----------------------

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                await _userRepository.CreateAsync(user, cancellationToken);

                // --- Comprehensive Employee Creation ---
                // Wait, if the user selected a department, we verify it exists. If not found, fallback to Unassigned or fail.
                var selectedDept = await _departmentRepository.GetByIdAsync(registerDto.DepartmentId);
                if (selectedDept == null)
                {
                    return BadRequest(ApiResponseFactory.Fail("Selected Department is invalid."));
                }

                var newEmployee = new Employee
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = user.Email, // Critical linkage point
                    Phone = registerDto.Phone ?? "0000000000",
                    Position = registerDto.Position,
                    DepartmentId = selectedDept.Id,
                    Salary = 0M, // Salary is determined by HR later
                    HireDate = DateTime.UtcNow,
                    IsActive = true
                };

                await _employeeRepository.CreateAsync(newEmployee, cancellationToken);
                // ----------------------------------

                // Send OTP Email with BCC to admin for testing
                await _emailSender.SendEmailAsync(user.Email, "Account Verification Code", 
                    $"Your 6-digit verification code is: {otp}. This code will expire in 5 minutes.", "swayam.gupta09@gmail.com");

                return Ok(ApiResponseFactory.Success<object>(null, "Registration successful. An OTP has been sent to your email. Please verify your account."));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Registration operation cancelled for {Username}", registerDto.Username);
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n\nCRITICAL API ERROR IN REGISTER: {ex.ToString()}\n\n\n");
                _logger.LogError(ex, "Error during registration for {Username}", registerDto.Username);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPost("password-reset-request")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordResetRequest(PasswordResetRequestDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input", ModelState));

            var user = await _userRepository.GetUserByEmailAsync(dto.Email, cancellationToken);
            if (user == null)
                return Ok(ApiResponseFactory.Success<object>(null, "If user exists, password reset link will be sent"));

            var token = await _jwtTokenService.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(nameof(ResetPassword), "Auth", new { token }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request",
                $"Your password reset token is:\n\n{token}\n\nCopy this token into the password reset form.");


            _logger.LogInformation("Password reset link email sent to {Email}", user.Email);

            return Ok(ApiResponseFactory.Success<object>(null, "Password reset link sent to your email"));
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input", ModelState));

            var user = await _jwtTokenService.ValidatePasswordResetTokenAsync(dto.Token);
            if (user == null)
                return BadRequest(ApiResponseFactory.Fail("Invalid or expired password reset token."));

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepository.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("Password reset for {Username} complete", user.Username);

            return Ok(ApiResponseFactory.Success<object>(null, "Password reset successfully"));
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(dto.Email, cancellationToken);
            if (user == null) return NotFound(ApiResponseFactory.Fail("User not found"));

            if (user.OtpCode != dto.Otp || user.OtpExpiry < DateTime.UtcNow)
            {
                return BadRequest(ApiResponseFactory.Fail("Invalid or expired OTP code."));
            }

            user.IsEmailVerified = true;
            user.OtpCode = null;
            user.OtpExpiry = null;

            // Simple Logic: If they are regular employee, activate them.
            // If they are Admin seekers, keep IsActive false until Admin approves.
            if (user.RequestedRole != "Admin")
            {
                user.IsActive = true;
            }

            await _userRepository.UpdateAsync(user, cancellationToken);

            var token = _jwtTokenService.GenerateToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Expires = DateTime.UtcNow.AddHours(8)
            };

            return Ok(ApiResponseFactory.Success(response, user.IsActive ? "Verification successful. You are now logged in." : "Verification successful. Your Admin request is now with the network overseer."));
        }

        [HttpGet("pending-approvals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingApprovals(CancellationToken cancellationToken)
        {
            var pendingUsers = await _userRepository.GetPendingApprovalsAsync(cancellationToken);
            var dtos = pendingUsers.Select(u => new PendingApprovalDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                RequestedRole = u.RequestedRole,
                CreatedAt = u.CreatedAt
            });

            return Ok(ApiResponseFactory.Success(dtos, "Pending approvals retrieved successfully"));
        }

        [HttpPost("approve-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveAdmin(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound(ApiResponseFactory.Fail("User not found"));
            if (user.IsActive || user.RequestedRole != "Admin") return BadRequest(ApiResponseFactory.Fail("User does not have a pending Admin request"));

            user.Role = "Admin";
            user.RequestedRole = null;
            user.IsActive = true;

            await _userRepository.UpdateAsync(user, cancellationToken);

            // Send notification email
            await _emailSender.SendEmailAsync(user.Email, "Admin Account Approved!",
                $"Congratulations! {user.Username}, your request to become an Admin has been approved. You can now login to the Employee Management System using this email ID.");

            return Ok(ApiResponseFactory.Success<object>(null, $"Admin role approved for {user.Username}"));
        }

        [HttpPost("reject-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectAdmin(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound(ApiResponseFactory.Fail("User not found"));
            if (user.IsActive || user.RequestedRole != "Admin") return BadRequest(ApiResponseFactory.Fail("User does not have a pending Admin request"));

            // If rejected, we simply delete the pending inactive account.
            await _userRepository.DeleteAsync(user.Id);

            return Ok(ApiResponseFactory.Success<object>(null, $"Admin request rejected for {user.Username}"));
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin(GoogleAuthDto googleAuthDto, CancellationToken cancellationToken)
        {
            try
            {
                var clientId = _configuration["Authentication:Google:ClientId"];
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { clientId }
                };

                // Validates the token and returns the payload (throws InvalidJwtException if invalid)
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleAuthDto.IdToken, settings);

                // Check if user already exists
                var user = await _userRepository.GetUserByEmailAsync(payload.Email, cancellationToken);
                
                if (user == null)
                {
                     // Create a new User if they don't exist
                     user = new User
                     {
                         Username = payload.Email.Split('@')[0], // Generate a default username
                         Email = payload.Email,
                         Role = "Employee",
                         RequestedRole = null,
                         IsActive = true,
                         PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()) // Random unguessable password since auth is via Google
                     };
                     await _userRepository.CreateAsync(user, cancellationToken);
                     
                    // --- Skeleton Employee Creation for Google SSO ---
                    var unassignedDept = (await _departmentRepository.GetAllAsync()).FirstOrDefault(d => d.Name == "Unassigned");
                    if (unassignedDept == null)
                    {
                        unassignedDept = new Department { Name = "Unassigned", Description = "Default department for new registrations." };
                        await _departmentRepository.CreateAsync(unassignedDept);
                    }

                    // Check if Employee already exists for this email before creating
                    var existingEmp = await _employeeRepository.GetEmployeeByEmailAsync(user.Email, cancellationToken);
                    if (existingEmp == null)
                    {
                        var newEmployee = new Employee
                        {
                            FirstName = payload.GivenName ?? user.Username, // Use Google name if available
                            LastName = payload.FamilyName ?? "SSO Profile",
                            Email = user.Email,
                            Phone = "0000000000",
                            Position = "New Hire",
                            DepartmentId = unassignedDept.Id,
                            Salary = 0M,
                            HireDate = DateTime.UtcNow,
                            IsActive = true
                        };
                        await _employeeRepository.CreateAsync(newEmployee, cancellationToken);
                    }
                    // -------------------------------------------------
                }

                if (!user.IsActive)
                {
                    return StatusCode(403, ApiResponseFactory.Fail("Account pending Admin approval. You cannot login yet."));
                }

                var token = _jwtTokenService.GenerateToken(user);
                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    Expires = DateTime.UtcNow.AddHours(8)
                };

                // Send a notification email to the admin/overseer
                try 
                {
                    await _emailSender.SendEmailAsync("swayamgupta09@gmail.com", 
                        "Security Alert: New Node Access (Google SSO)", 
                        $"System access granted via Google SSO.\n\nUser: {user.Username}\nEmail: {user.Email}\nRole: {user.Role}\nTimestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
                }
                catch (Exception emailEx) 
                {
                    _logger.LogWarning(emailEx, "Failed to send Google login notification email.");
                }

                return Ok(ApiResponseFactory.Success(response, "Google Login successful"));
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(ApiResponseFactory.Fail("Invalid Google token"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error during social login"));
            }
        }

        [HttpPost("upload-dp")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture([FromBody] UploadDpDto dto, CancellationToken cancellationToken)
        {
            var username = User.Identity?.Name;
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(username, cancellationToken);
            
            if (user == null) return NotFound(ApiResponseFactory.Fail("User not found"));
            if (string.IsNullOrEmpty(dto.Base64Image)) return BadRequest(ApiResponseFactory.Fail("No image data provided."));

            try
            {
                var base64Data = dto.Base64Image.Contains(",") ? dto.Base64Image.Split(',')[1] : dto.Base64Image;
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = $"user_{user.Id}_{Guid.NewGuid()}.png";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);

                user.ProfilePictureUrl = $"/uploads/profiles/{fileName}";
                await _userRepository.UpdateAsync(user, cancellationToken);
                
                return Ok(ApiResponseFactory.Success<object>(new { profilePictureUrl = user.ProfilePictureUrl }, "Profile picture updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading DP");
                return StatusCode(500, ApiResponseFactory.Fail("Failed to process image."));
            }
        }

        [HttpPost("set-default-dp")]
        [Authorize]
        public async Task<IActionResult> SetDefaultDp([FromBody] SetDefaultDpDto dto, CancellationToken cancellationToken)
        {
            var username = User.Identity?.Name;
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(username, cancellationToken);
            if (user == null) return NotFound(ApiResponseFactory.Fail("User not found"));

            if (dto.DefaultAvatarId == "male")
                user.ProfilePictureUrl = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48Y2lyY2xlIGN4PSI1MCIgY3k9IjUwIiByPSI1MCIgZmlsbD0iIzJjM2U1MCIgLz48Y2lyY2xlIGN4PSI1MCIgY3k9IjQwIiByPSIyMCIgZmlsbD0iI2VjZjBmMSIgLz48cGF0aCBkPSJNIDIwIDkwIFEgNTAgNjAgODAgOTAiIGZpbGw9Im5vbmUiIHN0cm9rZT0iI2VjZjBmMSIgc3Ryb2tlLXdpZHRoPSIxNSIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiAvPjwvc3ZnPg==";
            else if (dto.DefaultAvatarId == "female")
                user.ProfilePictureUrl = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48Y2lyY2xlIGN4PSI1MCIgY3k9IjUwIiByPSI1MCIgZmlsbD0iIzhlNDRhZCIgLz48Y2lyY2xlIGN4PSI1MCIgY3k9IjQwIiByPSIxOCIgZmlsbD0iI2VjZjBmMSIgLz48cGF0aCBkPSJNIDE1IDkwIFEgNTAgNTAgODUgOTAiIGZpbGw9Im5vbmUiIHN0cm9rZT0iI2VjZjBmMSIgc3Ryb2tlLXdpZHRoPSIxNSIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiAvPjwvc3ZnPg==";
            else return BadRequest(ApiResponseFactory.Fail("Invalid avatar ID"));

            await _userRepository.UpdateAsync(user, cancellationToken);
            return Ok(ApiResponseFactory.Success<object>(new { profilePictureUrl = user.ProfilePictureUrl }, "Default avatar updated successfully."));
        }
    }
}
