using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserInputValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserInputValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage(ResourceMessages.LoginPasswordRequired);

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage(ResourceMessages.CurrentPasswordRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword));

            RuleFor(x => x)
                .Must(x => x.CurrentPassword != x.NewPassword)
                .WithMessage(ResourceMessages.NewPasswordMustBeDifferent)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword) && !string.IsNullOrWhiteSpace(x.CurrentPassword));
        }
    }
}
