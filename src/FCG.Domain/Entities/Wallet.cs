namespace FCG.Domain.Entities
{
    public sealed class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public User? User { get; private set; }

        private Wallet(Guid userId)
        {
            UserId = userId;
        }

        private void AddInitialBalance()
        {
            Balance += 10;
        }

        public static Wallet Create(Guid userId)
        {
            var wallet = new Wallet(userId);
            wallet.AddInitialBalance();
            return wallet;
        }
    }
}