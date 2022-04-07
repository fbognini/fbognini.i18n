using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class ConfigurationConfiguration : IEntityTypeConfiguration<Entities.Configuration>
    {
        public void Configure(EntityTypeBuilder<Entities.Configuration> builder)
        {
            builder.ToTable(nameof(i18nContext.Configurations), "i18n");
            builder.HasKey(s => s.Code);
        }
    }
}
