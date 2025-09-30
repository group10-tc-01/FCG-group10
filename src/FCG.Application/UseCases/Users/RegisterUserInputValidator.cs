using FCG.Application.UseCases.UsersDTO;
using FluentValidation;

namespace FCG.Application.UseCases.Users.RegisterUser
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

                .Must(MeetsDomainPasswordRequirements)
                .WithMessage("A senha não atende aos requisitos de força (mínimo 8 caracteres, maiúscula, número, e caractere especial).");
        }

        private bool MeetsDomainPasswordRequirements(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            if (!password.Any(char.IsLetter))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
                return false;

            return true;
        }
    }
}
