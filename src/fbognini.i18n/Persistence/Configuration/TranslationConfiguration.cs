using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class TranslationConfiguration : IEntityTypeConfiguration<Entities.Translation>
    {
        public void Configure(EntityTypeBuilder<Entities.Translation> builder)
        {
            builder.ToTable(nameof(I18nContext.Translations), "i18n");
            builder.HasKey(s => new { s.LanguageId, s.TextId });

            builder.Property(x => x.Updated)
                .HasDefaultValue(new DateTime(1970, 1, 1));
        }
    }
}
