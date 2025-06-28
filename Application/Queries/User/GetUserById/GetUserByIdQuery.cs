using MediatR;
using Application.Dtos;

namespace Application.Queries.User.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;