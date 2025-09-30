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

            builder.Property(u => u.Name)
                .HasConversion(
                    name => name.Value,
                    value => Name.Create(value))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasConversion(
                    password => password.Value,
                    value => Password.FromHash(value))
                .HasMaxLength(255)
                .IsRequired();


            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
