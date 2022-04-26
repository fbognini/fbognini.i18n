using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class TextConfiguration : IEntityTypeConfiguration<Entities.Text>
    {
        public void Configure(EntityTypeBuilder<Entities.Text> builder)
        {
            builder.HasKey(s => new { s.Id });

            builder.Property(x => x.Id)
                .HasMaxLength(100);

            builder.Property(x => x.Group)
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);
        }
    }
}
