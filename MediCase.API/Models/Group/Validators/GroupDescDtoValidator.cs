using FluentValidation;
using MediCase.API.Models.Group;

namespace MediCase.API.Models.Group.Validators
{
    public class GroupDescDtoValidator : AbstractValidator<GroupDescDto>
    {
        public GroupDescDtoValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(255);
        }
    }
}
