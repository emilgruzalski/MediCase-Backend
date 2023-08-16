using FluentValidation;

namespace MediCase.WebAPI.Models.Account.Validators
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .MinimumLength(8)
                .MaximumLength(32);
            RuleFor(x => x.NewPassword)
                .MinimumLength(8)
                .MaximumLength(32);
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword);
        }
    }
}
