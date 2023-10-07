using FluentValidation;
using MediCase.API.Models.Group;

namespace MediCase.API.Models.Group.Validators
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
