using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class TextConfiguration : IEntityTypeConfiguration<Entities.Text>
    {
        public void Configure(EntityTypeBuilder<Entities.Text> builder)
        {
            builder.HasKey(s => new { s.TextId, s.ResourceId });

            builder.Property(x => x.TextId)
                .HasMaxLength(100);

            builder.Property(x => x.ResourceId)
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);
        }
    }
}
