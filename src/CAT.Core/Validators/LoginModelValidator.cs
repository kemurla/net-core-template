using CAT.Core.Models;

using FluentValidation;

namespace CAT.Core.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
