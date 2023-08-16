using FluentValidation;
using MediCase.WebAPI.Entities;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.Group;

namespace MediCase.WebAPI.Models.Group.Validators
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
