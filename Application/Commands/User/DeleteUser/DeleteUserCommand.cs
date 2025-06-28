using MediatR;

namespace Application.Commands.User.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Guid>;
