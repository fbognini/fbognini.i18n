using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fbognini.i18n.Persistence.Languages
{
    internal class LanguageConfiguration : IEntityTypeConfiguration<Entities.Language>
    {
        public void Configure(EntityTypeBuilder<Entities.Language> builder)
        {
            builder.ToTable(nameof(i18nContext.Languages), "i18n");
            builder.HasKey(s => s.Id);
        }
    }
}
