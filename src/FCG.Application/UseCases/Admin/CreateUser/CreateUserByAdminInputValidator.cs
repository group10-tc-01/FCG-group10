using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminInputValidator : AbstractValidator<CreateUserByAdminRequest>
    {
        public CreateUserByAdminInputValidator()
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

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Invalid role specified.");
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
