using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesInputValidator : AbstractValidator<GetAllGamesInput>
    {
        public GetAllGamesInputValidator()
        {
            When(x => x.Filter is not null, () =>
            {
                RuleFor(x => x.Filter.Name)
                    .MaximumLength(255)
                    .WithMessage(ResourceMessages.GameNameMaxLength);

                RuleFor(x => x.Filter.MinPrice)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Filter.MinPrice.HasValue)
                    .WithMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);

                RuleFor(x => x.Filter.MaxPrice)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Filter.MaxPrice.HasValue)
                    .WithMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);

                RuleFor(x => x.Filter.MaxPrice)
                    .GreaterThanOrEqualTo(x => x.Filter.MinPrice!.Value)
                    .When(x => x.Filter.MinPrice.HasValue && x.Filter.MaxPrice.HasValue)
                    .WithMessage(ResourceMessages.GameMaxPriceMustBeGreaterThanMinPrice);
            });
        }
    }
}

