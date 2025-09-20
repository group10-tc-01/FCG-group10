using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class GameConfiguration : BaseConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable("Games");

            builder.Property(g => g.Name)
                .HasConversion(
                    name => name.Value,
                    value => Name.Create(value)
                    )
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(g => g.Description)
                .HasMaxLength(1000);

            builder.Property(g => g.Price)
                .HasConversion(
                    price => price.Value,
                    value => Price.Create(value)
                    )
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(g => g.Category)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(g => g.Name)
                .HasDatabaseName("IX_Games_Name");

            builder.HasIndex(g => g.Category)
                .HasDatabaseName("IX_Games_Category");

            builder.HasIndex(g => g.Price)
                .HasDatabaseName("IX_Games_Price");

            builder.HasIndex(g => new { g.Category, g.Price, g.IsActive })
                .HasDatabaseName("IX_Games_Category_Price_Active")
                .HasFilter("IsActive = 1");

            builder.HasIndex(g => new { g.Name, g.Category })
                .HasDatabaseName("IX_Games_Name_Category");
        }
    }
}
