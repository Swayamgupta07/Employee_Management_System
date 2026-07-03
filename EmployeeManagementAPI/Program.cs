using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeManagementAPI.Configurations;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Middleware;
using EmployeeManagementAPI.Repositories.Implementations;
using EmployeeManagementAPI.Repositories.Interfaces;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using TimeSpanToStringConverter = EmployeeManagementAPI.Configurations.TimeSpanToStringConverter;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/employee-management-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Employee Management API");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();

    builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
    builder.Services.AddScoped<IReportRepository, ReportRepository>();

    builder.Services.AddScoped<IBonusFeatureRepository, BonusFeatureRepository>();

    builder.Services.AddSingleton<IEmailSender>(provider =>
    new SmtpEmailSenderRepository(
        host: "smtp.gmail.com",
        port: 587,
        fromEmail: "swayamgupta09@gmail.com",
        password: "jujo fdau zise velp"
    ));


    builder.Services.AddControllers();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });
    
    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
    
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var username = context.Principal?.Identity?.Name;
                var role = context.Principal?.FindFirst(ClaimTypes.Role)?.Value;
                Console.WriteLine($"Token validated! User: {username}, Role: {role}");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"JWT Challenge triggered: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });
    builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter());
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("HROnly", policy => policy.RequireRole("HR", "Admin"));
        options.AddPolicy("EmployeeAccess", policy => policy.RequireRole("Employee", "HR", "Admin"));
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "http://localhost:3001",
                "https://localhost:3001",
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Employee Management API",
            Description = "A comprehensive API for managing employees, departments, and attendance",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "support@company.com"
            }
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer abc123def456\""
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>("database");

    // Response Compression
    builder.Services.AddResponseCompression();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
            options.RoutePrefix = string.Empty;
        });
    }
    else
    {
        app.UseHsts();
    }

    // Global Error Handling Middleware (must be first)
    app.UseMiddleware<ErrorHandlingMiddleware>();

    // Security Headers
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
        await next();
    });

    // app.UseHttpsRedirection();
    app.UseResponseCompression();
    app.UseStaticFiles();

    app.UseCors("AllowReactApp");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    // Database Migration and Seeding
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Starting database migration...");
            
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Applying pending migrations...");
                context.Database.Migrate();
                logger.LogInformation("Database migration completed successfully");
            }
            else
            {
                logger.LogInformation("Database is up to date - no migrations needed");
            }

            context.Database.EnsureCreated();
            
            logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
            throw;
        }
    }

    Log.Information("Employee Management API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}