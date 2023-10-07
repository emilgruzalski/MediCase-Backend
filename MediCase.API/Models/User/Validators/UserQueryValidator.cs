using FluentValidation;
using MediCase.API.Entities;

namespace MediCase.API.Models.User.Validators
{
    public class UserQueryValidator : AbstractValidator<UserQuery>
    {
        private int[] allowedPageSizes = new[] { 10, 15, 30 };

        private string[] allowedSortByColumnNames =
            {nameof(Entities.Admin.User.FirstName), nameof(Entities.Admin.User.LastName), nameof(Entities.Admin.User.Email),};
        public UserQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
               .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
