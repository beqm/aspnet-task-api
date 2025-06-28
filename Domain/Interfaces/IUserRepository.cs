using User = Domain.Models.User;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetRangeAsync(int page, int pageSize);
    Task CreateUserAsync(User user);
    Task ChangePasswordAsync(Guid id, string newPasswordHash);
    Task DeleteUserAsync(Guid id);
    Task<User?> GetUserWithTasksAsync(Guid id);
}
