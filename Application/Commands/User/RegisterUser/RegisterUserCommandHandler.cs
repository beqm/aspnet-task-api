using MediatR;
using Domain.Interfaces;
using Application.Common.Exceptions;
using UserModel = Domain.Models.User;

namespace Application.Commands.User.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid?>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IAuthenticationService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Guid?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByUsernameAsync(request.Username);
        if (existing != null)
        {    
            throw new ConflictException($"Username '{request.Username}' is already taken.");
        }

        var hashedPassword = _authService.HashPassword(request.Password);
        var user = UserModel.Create(request.Username, hashedPassword, request.Role);

        await _userRepository.CreateUserAsync(user);
        return user.Id;
    }
}
