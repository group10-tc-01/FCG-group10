using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Persistance.Configuration
{
    [ExcludeFromCodeCoverage]
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

            builder.OwnsOne(lg => lg.PurchasePrice, priceBuilder =>
            {
                priceBuilder.Property(p => p.Value)
                    .HasColumnName("PurchasePrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

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