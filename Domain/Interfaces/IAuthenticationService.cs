using User = Domain.Models.User;

namespace Domain.Interfaces;

public interface IAuthenticationService
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
    string GenerateJwtToken(User user);
}