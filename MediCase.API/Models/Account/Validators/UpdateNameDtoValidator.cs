using FluentValidation;
using MediCase.API.Models.Account;

namespace MediCase.API.Models.Account.Validators
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
