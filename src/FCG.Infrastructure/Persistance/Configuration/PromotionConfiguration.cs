using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Persistance.Configuration
{
    [ExcludeFromCodeCoverage]
    public class PromotionConfiguration : BaseConfiguration<Promotion>
    {
        public override void Configure(EntityTypeBuilder<Promotion> builder)
        {
            base.Configure(builder);

            builder.ToTable("Promotions");

            builder.OwnsOne(p => p.Discount, discountBuilder =>
            {
                discountBuilder.Property(d => d.Value)
                    .HasColumnName("Discount")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();
            });

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