using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class WalletBuilder
    {
        public static Wallet Build(Guid? userId = null, decimal balance = 10m)
        {
            var effectiveUserId = userId ?? Guid.NewGuid();
            var wallet = Wallet.Create(effectiveUserId);
            
            if (balance != 10m)
            {
                // Adiciona o balance usando reflection se necessário
                var balanceProperty = typeof(Wallet).GetProperty("Balance");
                if (balanceProperty != null && balanceProperty.CanWrite)
                {
                    balanceProperty.SetValue(wallet, balance);
                }
            }
            
            return wallet;
        }
    }
}
