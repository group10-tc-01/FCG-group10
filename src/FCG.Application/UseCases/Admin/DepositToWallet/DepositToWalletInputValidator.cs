using FCG.Messages;
using FluentValidation;

namespace FCG.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletInputValidator : AbstractValidator<DepositToWalletRequest>
    {
        public DepositToWalletInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.WalletId)
                .NotEmpty()
                .WithMessage("Wallet ID is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Deposit amount must be greater than zero.")
                .LessThanOrEqualTo(1000000)
                .WithMessage("Deposit amount cannot exceed 1,000,000.");
        }
    }
}
