using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class ConfigurationConfiguration : IEntityTypeConfiguration<Entities.Configuration>
    {
        public void Configure(EntityTypeBuilder<Entities.Configuration> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(5)
                .IsFixedLength();

            builder.Property(x => x.BaseUriResource)
                .HasMaxLength(200);
        }
    }
}
