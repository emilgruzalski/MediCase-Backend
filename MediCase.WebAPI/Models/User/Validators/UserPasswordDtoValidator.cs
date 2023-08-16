using FluentValidation;

namespace MediCase.WebAPI.Models.User.Validators
{
    public class UserPasswordDtoValidator : AbstractValidator<UserPasswordDto>
    {
        public UserPasswordDtoValidator()
        {
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }
}
