using FluentValidation;

namespace Application.Commands.Task.UpdateTask;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .Must(HasAtLeastOneFieldToUpdate)
            .WithMessage("At least one field to update must be provided.");
    }

    private bool HasAtLeastOneFieldToUpdate(UpdateTaskCommand cmd)
    {
        return cmd.Title != null || cmd.Description != null || cmd.Complete.HasValue;
    }
}