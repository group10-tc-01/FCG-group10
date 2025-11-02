using FCG.Application.UseCases.Admin.DepositToWallet;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Admin.DepositToWallet
{
    public static class DepositToWalletInputBuilder
    {
        public static DepositToWalletRequest Build()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                Amount = 100.00m
            };
        }

        public static DepositToWalletRequest BuildWithAmount(decimal amount)
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                Amount = amount
            };
        }

        public static DepositToWalletRequest BuildWithUserIdAndWalletId(Guid userId, Guid walletId)
        {
            return new DepositToWalletRequest
            {
                UserId = userId,
                WalletId = walletId,
                Amount = 100.00m
            };
        }

        public static DepositToWalletRequest BuildWithZeroAmount()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                Amount = 0m
            };
        }

        public static DepositToWalletRequest BuildWithNegativeAmount()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                Amount = -50.00m
            };
        }

        public static DepositToWalletRequest BuildWithExcessiveAmount()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                Amount = 2000000m // Maior que o limite de 1.000.000
            };
        }

        public static DepositToWalletRequest BuildWithEmptyUserId()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.Empty,
                WalletId = Guid.NewGuid(),
                Amount = 100.00m
            };
        }

        public static DepositToWalletRequest BuildWithEmptyWalletId()
        {
            return new DepositToWalletRequest
            {
                UserId = Guid.NewGuid(),
                WalletId = Guid.Empty,
                Amount = 100.00m
            };
        }

        public static DepositToWalletRequest BuildWithMismatchedWallet(Guid userId, Guid wrongWalletId)
        {
            return new DepositToWalletRequest
            {
                UserId = userId,
                WalletId = wrongWalletId,
                Amount = 100.00m
            };
        }
    }
}
