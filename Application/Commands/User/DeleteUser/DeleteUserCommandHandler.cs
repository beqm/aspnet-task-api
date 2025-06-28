using MediatR;
using Domain.Interfaces;

namespace Application.Commands.User.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUserAsync(request.Id);
        return request.Id;
    }
}
