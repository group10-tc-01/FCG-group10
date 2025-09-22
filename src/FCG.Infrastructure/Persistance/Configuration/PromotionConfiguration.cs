using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class PromotionConfiguration : BaseConfiguration<Promotion>
    {
        public override void Configure(EntityTypeBuilder<Promotion> builder)
        {
            base.Configure(builder);

            builder.ToTable("Promotions");

            builder.Property(p => p.Discount)
                   .HasConversion(
                        discount => discount.Value,
                        value => Discount.Create(value))
                   .HasColumnName("Discount")
                   .HasColumnType("decimal(5,2)")
                   .IsRequired();

            builder.Property(e => e.StartDate)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(e => e.EndDate)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(e => e.GameId)
                .IsRequired();

            builder.HasOne(e => e.Game)
                .WithMany(g => g.Promotions)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

