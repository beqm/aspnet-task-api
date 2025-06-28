using MediatR;
using Domain.Interfaces;


namespace Application.Commands.User.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;

    public ChangePasswordHandler(IUserRepository userRepository, IAuthenticationService authService)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var hashed = _authService.HashPassword(request.NewPassword);
        await _userRepository.ChangePasswordAsync(request.Id, hashed);
        return request.Id;
    }
}
