using CAT.Core.Models;
using FluentValidation;

namespace CAT.Core.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
