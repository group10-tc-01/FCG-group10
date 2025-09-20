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
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //.HasDefaultValueSql("GETUTCDATE()"); // SQL Server
            // .HasDefaultValueSql("NOW()"); // Para PostgreSQL
            // .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Para MySQL

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            //.HasDefaultValueSql("GETUTCDATE()"); // SQL Server
            // .HasDefaultValueSql("NOW()"); // Para PostgreSQL
            // .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Para MySQL
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName($"IX_{typeof(T).Name}_IsActive");

            builder.HasIndex(e => e.CreatedAt)
                .HasDatabaseName($"IX_{typeof(T).Name}_CreatedAt");

        }
    }
}
