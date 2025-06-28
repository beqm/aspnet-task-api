using MediatR;
using Application.Dtos;

namespace Application.Queries.User.GetUsersRange;

public record GetUsersRangeQuery(int Page, int PageSize) : IRequest<IEnumerable<UserDto>>;