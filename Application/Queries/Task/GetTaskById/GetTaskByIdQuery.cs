using MediatR;
using Application.Dtos;

namespace Application.Queries.Task.GetTasksById;

public record GetTaskByIdQuery(Guid Id, Guid UserId, string Role) : IRequest<TaskDto?>;
