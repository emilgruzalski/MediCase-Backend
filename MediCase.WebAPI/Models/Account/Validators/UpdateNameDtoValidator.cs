using FluentValidation;

namespace MediCase.WebAPI.Models.Account.Validators
{
    public class UpdateNameDtoValidator : AbstractValidator<UpdateNameDto>
    {
        public UpdateNameDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
