using FluentValidation;

namespace MediCase.WebAPI.Models.Group.Validators
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
