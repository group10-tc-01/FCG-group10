using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<LibraryGame> LibraryGames { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Library)
                .WithOne(l => l.User)
                .HasForeignKey<Library>(l => l.UserId);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Promotions)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId);

            modelBuilder.Entity<LibraryGame>()
                .HasKey(lg => new { lg.LibraryId, lg.GameId });

            modelBuilder.Entity<LibraryGame>()
                .HasOne(lg => lg.Library)
                .WithMany(l => l.LibraryGames)
                .HasForeignKey(lg => lg.LibraryId);

            modelBuilder.Entity<LibraryGame>()
                .HasOne(lg => lg.Game)
                .WithMany(g => g.LibraryGames)
                .HasForeignKey(lg => lg.GameId);

            modelBuilder.Entity<Wallet>()
                .HasIndex(w => w.UserId);

            modelBuilder.Entity<Library>()
                .HasIndex(l => l.UserId);

            modelBuilder.Entity<Promotion>()
                .HasIndex(p => p.GameId);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Email);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Password);

            modelBuilder.Entity<Game>()
                .OwnsOne(g => g.Price);
        }
    }
}