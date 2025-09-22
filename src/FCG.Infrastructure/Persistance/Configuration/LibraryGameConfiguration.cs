using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class LibraryGameConfiguration : BaseConfiguration<LibraryGame>
    {
        public override void Configure(EntityTypeBuilder<LibraryGame> builder)
        {
            base.Configure(builder);

            builder.ToTable("LibraryGames");

            builder.HasKey(lg => new { lg.LibraryId, lg.GameId });

            builder.Property(lg => lg.PurchaseDate)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(lg => lg.PurchasePrice)
                .HasConversion(
                    price => price.Value,
                    value => Price.Create(value))
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(lg => lg.Library)
                .WithMany(l => l.LibraryGames)
                .HasForeignKey(lg => lg.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lg => lg.Game)
                .WithMany(g => g.LibraryGames)
                .HasForeignKey(lg => lg.GameId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
