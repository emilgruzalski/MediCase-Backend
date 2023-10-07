using FluentValidation;
using MediCase.API.Entities;

namespace MediCase.API.Models.Group.Validators
{
    public class GroupQueryValidator : AbstractValidator<GroupQuery>
    {
        private int[] allowedPageSizes = new[] { 10, 15, 30 };

        private string[] allowedSortByColumnNames =
            {nameof(Entities.Admin.Group.Name),};
        public GroupQueryValidator()
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
