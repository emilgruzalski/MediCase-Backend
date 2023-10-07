using FluentValidation;
using MediCase.API.Entities.Admin;
using MediCase.API.Entities;

namespace MediCase.API.Models.User.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator(MediCaseAdminContext dbContext)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(40)
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email is already in use");
                    }
                });

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }
}
