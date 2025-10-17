using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Users.Register
{
    public class RegisterUserInputValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ResourceMessages.NameRequired)
                .MaximumLength(100)
                .WithMessage(ResourceMessages.NameIsLong);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ResourceMessages.LoginEmalRequired)
                .EmailAddress()
                .WithMessage(ResourceMessages.LoginInvalidEmailFormat);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ResourceMessages.LoginPasswordRequired)
                .MaximumLength(100)
                .WithMessage(ResourceMessages.LongPassword)

                .Must(BeValidPassword)
                .WithMessage(ResourceMessages.Password);
        }

        private static bool BeValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                Password.Create(password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
