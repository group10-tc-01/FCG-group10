using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesInputValidator : AbstractValidator<GetAllGamesInput>
    {
        public GetAllGamesInputValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage(ResourceMessages.GameNameMaxLength);

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue)
                .WithMessage(ResourceMessages.GameMaxPriceMustBeGreaterThanMinPrice);
        }
    }
}

