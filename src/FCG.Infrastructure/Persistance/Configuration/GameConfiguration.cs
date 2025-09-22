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
                    value => Name.Create(value))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(g => g.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(g => g.Price)
                .HasConversion(
                    price => price.Value,
                    value => Price.Create(value))
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(g => g.Category)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
