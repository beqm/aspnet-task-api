using MediatR;
using Application.Dtos;

namespace Application.Commands.Task.UpdateTask;

public record UpdateTaskCommand(
    Guid Id,
    Guid RequestingUserId,
    string? RequestingRole,
    string? Title = null,
    string? Description = null,
    bool? Complete = null
) : IRequest<TaskDto>;