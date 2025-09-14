using FCG.Domain.EntityGame;
using FCG.Domain.EntityUser;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.ContextDb
{
    public class FGCDbContext : DbContext
    {
        public FGCDbContext(DbContextOptions<FGCDbContext> options) : base(options)
        {
        }

        public DbSet<EntityUser> Users { get; set; }
        public DbSet<EntityGame> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityUser>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<EntityGame>()
                .HasKey(g => g.Id);
        }
    }
}

