using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistance.Configuration
{
    public class ExampleConfiguration : BaseConfiguration<Example>
    {
        public override void Configure(EntityTypeBuilder<Example> builder)
        {
            base.Configure(builder);

            builder.ToTable("Examples");

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
