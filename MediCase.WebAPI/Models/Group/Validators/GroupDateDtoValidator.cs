using FluentValidation;

namespace MediCase.WebAPI.Models.Group.Validators
{
    public class GroupDateDtoValidator : AbstractValidator<GroupDateDto>
    {
        public GroupDateDtoValidator()
        {
            RuleFor(x => x.ExpirationDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
