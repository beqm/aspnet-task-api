using FluentValidation;

namespace Application.Queries.User.GetUserWithTasks;

public class GetUserWithTasksQueryValidator : AbstractValidator<GetUserWithTasksQuery>
{
    public GetUserWithTasksQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
