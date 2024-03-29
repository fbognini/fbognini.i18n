﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace fbognini.i18n.Persistence.Configurations
{
    internal class TranslationConfiguration : IEntityTypeConfiguration<Entities.Translation>
    {
        public void Configure(EntityTypeBuilder<Entities.Translation> builder)
        {
            builder.HasKey(s => new { s.LanguageId, s.TextId, s.ResourceId });

            builder.Property(x => x.LanguageId)
                .HasMaxLength(5)
                .IsFixedLength();

            builder.Property(x => x.TextId)
                .HasMaxLength(100);

            builder.Property(x => x.ResourceId)
                .HasMaxLength(50);

            builder.Property(x => x.Updated)
                .HasDefaultValue(new DateTime(1970, 1, 1));

            builder.HasOne(x => x.Language)
                .WithMany(x => x.Translations)
                .HasForeignKey(s => new { s.LanguageId });

            builder.HasOne(x => x.Text)
                .WithMany(x => x.Translations)
                .HasForeignKey(s => new { s.TextId, s.ResourceId });
        }
    }
}
