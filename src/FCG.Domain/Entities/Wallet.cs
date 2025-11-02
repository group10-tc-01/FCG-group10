using FCG.Domain.Exceptions;

namespace FCG.Domain.Entities
{
    public sealed class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public User? User { get; }

        private Wallet(Guid userId)
        {
            UserId = userId;
        }

        private void AddInitialBalance()
        {
            Balance += 10;
        }

        public void Debit(decimal amount)
        {
            if (amount > Balance)
            {
                throw new DomainException("Insufficient balance in the wallet.");
            }

            Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new DomainException("Deposit amount must be greater than zero.");
            }

            Balance += amount;
        }

        public static Wallet Create(Guid userId)
        {
            var wallet = new Wallet(userId);
            wallet.AddInitialBalance();
            return wallet;
        }
    }
}