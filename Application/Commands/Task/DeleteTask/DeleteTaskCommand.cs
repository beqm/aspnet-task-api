using MediatR;

namespace Application.Commands.Task.DeleteTask;

public record DeleteTaskCommand(Guid Id, Guid RequestingUserId, string RequestingRole) : IRequest<Guid>;
