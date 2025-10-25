using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class UserGameConfiguration : BaseConfiguration<UserGame>
    {
        public override void Configure(EntityTypeBuilder<UserGame> builder)
        {
            base.Configure(builder);

            builder.ToTable("UserGames");

            builder.HasKey(lg => new { lg.UserId, lg.GameId });

            builder.Property(ug => ug.PurchaseDate)
                .HasColumnType("datetime")
                .IsRequired();
            builder.Property(ug => ug.Status)
                .HasConversion<string>()
                .HasMaxLength(15)
                .IsRequired();

            builder.HasOne(ug => ug.User)
                .WithMany(u => u.UserGames)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
