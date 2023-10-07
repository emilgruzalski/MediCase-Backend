using FluentValidation;
using MediCase.API.Entities.Admin;
using MediCase.API.Entities;

namespace MediCase.API.Models.Group.Validators
{
    public class GroupDtoValidator : AbstractValidator<GroupDto>
    {
        public GroupDtoValidator(MediCaseAdminContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(40)
                .Custom((value, context) =>
                {
                    var nameInUse = dbContext.Groups.Any(u => u.Name == value);
                    if (nameInUse)
                    {
                        context.AddFailure("Name", "Name is already in use");
                    }
                });

            RuleFor(x => x.Description)
                .MaximumLength(140);

            RuleFor(x => x.ExpirationDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
