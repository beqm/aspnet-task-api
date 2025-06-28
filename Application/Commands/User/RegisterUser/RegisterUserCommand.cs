using MediatR;

namespace Application.Commands.User.RegisterUser;

public record RegisterUserCommand(string Username, string Password, string Role) : IRequest<Guid?>;