using FluentValidation;
using MediCase.API.Entities.Admin;
using MediCase.API.Models.Group;
using Microsoft.EntityFrameworkCore;

namespace MediCase.API.Models.Group.Validators
{
    public class GroupNameDtoValidator : AbstractValidator<GroupNameDto>
    {
        public GroupNameDtoValidator(MediCaseAdminContext dbContext)
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
        }
    }
}
