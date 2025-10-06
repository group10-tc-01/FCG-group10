using FCG.Domain.Entities;
using FCG.Infrastructure.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance
{
    public class FcgDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<LibraryGame> LibraryGames { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public FcgDbContext(DbContextOptions<FcgDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameConfiguration).Assembly);
        }
    }
}