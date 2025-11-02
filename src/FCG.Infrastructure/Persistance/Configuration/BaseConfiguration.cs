using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Persistance.Configuration
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
