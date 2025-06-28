using MediatR;

namespace Application.Commands.User.LoginUser;

public record LoginUserCommand(string Username, string Password) : IRequest<string?>;