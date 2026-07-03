using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> action, string operation)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while {Operation}", operation);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
            }, "fetching all users");
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            }, $"fetching user by id {id}");
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            await ExecuteSafeAsync(async () =>
            {
                user.CreatedAt = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }, $"creating user {user.Username}");
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await ExecuteSafeAsync(async () =>
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }, $"updating user {user.Id}");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await ExecuteSafeAsync(async () =>
            {
                var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return true;
            }, $"deleting user {id}");
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
            }, $"checking if user {id} exists");
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string username, CancellationToken cancellationToken)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower()
                                           || u.Email.ToLower() == username.ToLower(), cancellationToken);
            }, $"fetching user by username or email {username}");
        }


        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
            }, $"fetching user by email {email}");
        }

        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower(), cancellationToken);
            }, $"checking if username {username} exists");
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
            }, $"checking if email {email} exists");
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await GetAllAsync(CancellationToken.None); // Or default
        }

        public async Task<IEnumerable<User>> GetPendingApprovalsAsync(CancellationToken cancellationToken)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Users.AsNoTracking()
                    .Where(u => u.IsActive == false && u.RequestedRole == "Admin")
                    .ToListAsync(cancellationToken);
            }, "fetching pending admin approvals");
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await GetByIdAsync(id, CancellationToken.None);
        }

        public async Task CreateAsync(User entity)
        {
            await CreateAsync(entity, CancellationToken.None);
        }

        public async Task UpdateAsync(User entity)
        {
            await UpdateAsync(entity, CancellationToken.None);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(id, CancellationToken.None);
        }

    }
}