using FluentValidation;

namespace Application.Commands.Task.DeleteTask;

public class DeleteTaskValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required for deletion.");
    }
}
