using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users");

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasConversion(
                       name => name.Value,
                       value => Name.Create(value));

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasConversion(
                       email => email.Value,
                       value => Email.Create(value));

            builder.Property(e => e.Password)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasConversion(
                       password => password.Value,
                       value => Password.Create(value));

            builder.Property(e => e.Role)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.HasIndex(e => e.Email)
                   .IsUnique();
        }
    }
}
