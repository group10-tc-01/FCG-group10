using FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO;
using FCG.Domain.Enum;
using FluentValidation;

namespace FCG.Application.UseCases.Users.RoleManagement
{
    public class RoleManagementInputValidator : AbstractValidator<RoleManagementRequest>
    {
        public RoleManagementInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("O Id do usuário não pode ser nulo ou vazio.");

            RuleFor(x => x.NewRole)
                .Must(role => Enum.IsDefined(typeof(Role), role))
                .WithMessage("O cargo do usuário é inválido.");
        }
    }
}
