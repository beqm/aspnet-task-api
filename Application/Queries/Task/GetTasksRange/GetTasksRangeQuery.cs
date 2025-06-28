using MediatR;
using Application.Dtos;

namespace Application.Queries.Task.GetTasksRange;

public record GetTasksRangeQuery(int Page, int PageSize, Guid UserId, string Role) : IRequest<IEnumerable<TaskDto>>;

