using FCG.Domain.Enum;
using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public class RoleManagementInputValidator : AbstractValidator<RoleManagementRequest>
    {
        public RoleManagementInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(ResourceMessages.UserIdRequired);

            RuleFor(x => x.NewRole)
                .Must(role => Enum.IsDefined(typeof(Role), role))
                .WithMessage(ResourceMessages.InvalidUserRole);
        }
    }
}
