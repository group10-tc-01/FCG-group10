using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Persistance.Configuration
{
    [ExcludeFromCodeCoverage]
    public class GameConfiguration : BaseConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable("Games", t => t.HasCheckConstraint("CK_Games_Price_GreaterThanZero", "Price > 0"));

            builder.OwnsOne(g => g.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(255)
                    .IsRequired();

                nameBuilder.HasIndex(n => n.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Games_Name_Unique");
            });

            builder.Property(g => g.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.OwnsOne(g => g.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Value)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            builder.Property(g => g.Category)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}