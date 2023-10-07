using FluentValidation;

namespace MediCase.API.Models.Account.Validators
{
    public class UpdateEmailDtoValidator : AbstractValidator<UpdateEmailDto>
    {
        public UpdateEmailDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(40);

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }
}
