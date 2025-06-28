using MediatR;
using Application.Dtos;

namespace Application.Queries.User.GetUserWithTasks;

public record GetUserWithTasksQuery(Guid Id) : IRequest<UserWithTasksDto>;
