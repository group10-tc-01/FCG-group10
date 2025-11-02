using FCG.Domain.Enum;
using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Games.Register
{
    public class RegisterGameInputValidator : AbstractValidator<RegisterGameInput>
    {
        public RegisterGameInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ResourceMessages.GameNameIsRequired)
                .MaximumLength(255)
                .WithMessage(ResourceMessages.GameNameMaxLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(ResourceMessages.GameDescriptionIsRequired)
                .MaximumLength(500)
                .WithMessage(ResourceMessages.GameDescriptionMaxLength);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage(ResourceMessages.GameCategoryIsRequired)
                .Must(category => Enum.TryParse<GameCategory>(category, true, out _))
                .WithMessage(ResourceMessages.GameCategoryIsRequired);
        }
    }
}
