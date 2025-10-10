using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Persistance.Configuration
{
    [ExcludeFromCodeCoverage]
    public class LibraryConfiguration : BaseConfiguration<Library>
    {
        public override void Configure(EntityTypeBuilder<Library> builder)
        {
            base.Configure(builder);

            builder.ToTable("Libraries");

            builder.HasKey(e => e.UserId);
            builder.HasOne(e => e.User)
                .WithOne(u => u.Library)
                .HasForeignKey<Library>(e => e.UserId);
        }
    }
}