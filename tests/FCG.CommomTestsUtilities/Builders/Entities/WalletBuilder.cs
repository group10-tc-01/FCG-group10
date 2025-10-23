using Bogus;
using FCG.Domain.Entities;
using System;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class WalletBuilder
    {
        // public static Wallet Build()
        public static Wallet Build(Guid? userId = null, decimal balance = 10m)

        {
            return new Faker<Wallet>().CustomInstantiator(f => Wallet.Create(f.Random.Guid())).Generate();
        }
    }
}
