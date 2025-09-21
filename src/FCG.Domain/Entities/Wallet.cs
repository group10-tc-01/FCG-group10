using System;

namespace FCG.Domain.Entities
{
    public sealed class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }

        public User User { get; set; }

        public Wallet() { }

        private Wallet(Guid userId, decimal initialBalance)
        {
            UserId = userId;
            Balance = initialBalance;

        }
        public static Wallet Create(Guid userId, decimal initialBalance)
        {
            return new Wallet(userId, initialBalance);
        }
    }
}