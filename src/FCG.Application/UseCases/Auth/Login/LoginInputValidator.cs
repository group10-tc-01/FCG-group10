using FCG.Domain.ValueObjects;
using FluentValidation;

namespace FCG.Application.UseCases.Auth.Login
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .Must(BeValidEmail)
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
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
