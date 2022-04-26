using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fbognini.i18n.Persistence.Languages
{
    internal class LanguageConfiguration : IEntityTypeConfiguration<Entities.Language>
    {
        public void Configure(EntityTypeBuilder<Entities.Language> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(5)
                .IsFixedLength();

            builder.Property(s => s.Description)
                .HasMaxLength(100);
        }
    }
}
