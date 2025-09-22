using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class WalletConfiguration : BaseConfiguration<Wallet>
    {
        public override void Configure(EntityTypeBuilder<Wallet> builder)
        {
            base.Configure(builder);

            builder.ToTable("Wallets", t => t.HasCheckConstraint("CK_Wallets_Balance_NonNegative", "Balance >= 0"));

            builder.Property(e => e.Balance)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasDefaultValue(0.00m);

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}