using MediatR;

namespace Application.Commands.User.ChangePassword;

public record ChangePasswordCommand(Guid Id, string NewPassword) : IRequest<Guid>;