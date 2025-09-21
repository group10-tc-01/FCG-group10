using FCG.Domain.Entities;
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


            builder.OwnsOne(p => p.Discount, discount =>
            {
                discount.Property(d => d.Value)
                    .HasConversion(
                        value => value,
                        value => (decimal)value
                        )
                    .HasColumnName("DiscountValue")
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


            builder.HasIndex(p => new { p.StartDate, p.EndDate })
                .HasDatabaseName("IX_Promotions_DateRange");

            builder.HasIndex(p => new { p.GameId, p.StartDate, p.EndDate })
                .HasDatabaseName("IX_Promotions_Game_DateRange");

            builder.HasIndex(p => new { p.IsActive, p.StartDate, p.EndDate })
                .HasDatabaseName("IX_Promotions_Active_DateRange")
                .HasFilter("IsActive = 1");

            builder.HasOne(e => e.Game)
                .WithMany(g => g.Promotions)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

