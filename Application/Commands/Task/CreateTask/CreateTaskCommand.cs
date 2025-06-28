using MediatR;
using Application.Dtos;

namespace Application.Commands.Task.CreateTask;

public record CreateTaskCommand(Guid UserId, string Title, string Description) : IRequest<TaskDto>;