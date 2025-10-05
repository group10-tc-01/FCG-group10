using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Domain.ValueObjects;
using FluentValidation;

namespace FCG.Application.UseCases.Users.Register
{
    public class RegisterUserInputValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O nome não pode exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O email é obrigatório.")
                .EmailAddress()
                .WithMessage("O formato do email é inválido.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MaximumLength(100)
                .WithMessage("A senha não pode exceder 100 caracteres.")

                .Must(BeValidPassword)
                .WithMessage("A senha não atende aos requisitos de força (mínimo 8 caracteres, número, e caractere especial).");
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
