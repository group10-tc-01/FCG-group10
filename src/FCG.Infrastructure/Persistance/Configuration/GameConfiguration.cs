using FCG.Domain.Entities;
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

            builder.OwnsOne(g => g.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(255)
                    .IsRequired();
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
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}