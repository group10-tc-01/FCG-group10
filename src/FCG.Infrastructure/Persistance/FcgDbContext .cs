using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FCG.Infrastructure.Persistance
{
    [ExcludeFromCodeCoverage]
    public class FcgDbContext(DbContextOptions<FcgDbContext> options) : DbContext(options)
    {
        public DbSet<Example> Examples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
