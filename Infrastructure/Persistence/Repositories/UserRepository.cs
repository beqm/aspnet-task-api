using System.Text;
using Domain.Interfaces;
using User = Domain.Models.User;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return null;

        var inputPasswordHash = ComputeSha256Hash(password);
        return user.Password == inputPasswordHash ? user : null;
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToHexString(bytes);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetRangeAsync(int page, int pageSize)
    {
        return await _dbContext.Users.AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task ChangePasswordAsync(Guid id, string newPasswordHash)
    {
        try
        {
            var affected = await _dbContext
                .Users
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(user => user.Password, newPasswordHash));

            if (affected == 0)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var affected = await _dbContext
            .Users
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();

        if (affected == 0)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
    }

    public async Task<User?> GetUserWithTasksAsync(Guid userId)
    {
        return await _dbContext.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
