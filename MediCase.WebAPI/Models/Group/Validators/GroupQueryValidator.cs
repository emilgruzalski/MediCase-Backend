using FluentValidation;
using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Models.Group;

namespace MediCase.WebAPI.Models.Group.Validators
{
    public class GroupQueryValidator : AbstractValidator<GroupQuery>
    {
        private int[] allowedPageSizes = new[] { 10, 15, 30 };

        private string[] allowedSortByColumnNames =
            {nameof(MediCase.WebAPI.Entities.Admin.Group.Name),};
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
