using FluentValidation;

namespace Application.Queries.User.GetUsersRange;

public class GetUsersRangeQueryValidator : AbstractValidator<GetUsersRangeQuery>
{
    public GetUsersRangeQueryValidator()
    {
        RuleFor(x => x.Page)
            .NotEmpty().WithMessage("Page is required.")
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .NotEmpty().WithMessage("PageSize is required.")
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .Must((query, pageSize) => pageSize >= query.Page)
                .WithMessage("PageSize must be greater than or equal to Page.");
    }
}
