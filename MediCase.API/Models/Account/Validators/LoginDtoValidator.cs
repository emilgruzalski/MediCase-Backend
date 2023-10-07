using FluentValidation;

namespace MediCase.API.Models.Account.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(40)
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }
}
