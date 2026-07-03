using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsernameOrEmailAsync(string username, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetPendingApprovalsAsync(CancellationToken cancellationToken);
    }
}
