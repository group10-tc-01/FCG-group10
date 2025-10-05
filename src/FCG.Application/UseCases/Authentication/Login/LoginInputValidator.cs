using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Authentication.Login
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ResourceMessages.LoginEmalRequired)
                .Must(BeValidEmail)
                .WithMessage(ResourceMessages.LoginInvalidEmailFormat);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ResourceMessages.LoginPasswordRequired);
        }

        private static bool BeValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                Email.Create(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
