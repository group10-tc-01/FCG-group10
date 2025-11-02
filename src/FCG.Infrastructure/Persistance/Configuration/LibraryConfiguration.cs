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

            builder.Property(l => l.UserId)
                .IsRequired();

            builder.HasOne(l => l.User)
                .WithOne(u => u.Library)
                .HasForeignKey<Library>(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}