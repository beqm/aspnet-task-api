using MediatR;
using Domain.Interfaces;

namespace Application.Commands.User.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string?>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;

    public LoginUserCommandHandler(IUserRepository userRepository, IAuthenticationService authService)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !_authService.VerifyPassword(user.Password, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        return _authService.GenerateJwtToken(user);
    }
}
