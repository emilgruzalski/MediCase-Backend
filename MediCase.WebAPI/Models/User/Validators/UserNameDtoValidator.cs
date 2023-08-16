using FluentValidation;
using MediCase.WebAPI.Entities.Admin;

namespace MediCase.WebAPI.Models.User.Validators
{
    public class UserNameDtoValidator : AbstractValidator<UserNameDto>
    {
        public UserNameDtoValidator(MediCaseAdminContext dbContext)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(40);
        }
    }
}
