﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using fbognini.i18n.Persistence;

#nullable disable

namespace fbognini.i18n.Persistence.Migrations
{
    [DbContext(typeof(I18nContext))]
    [Migration("20220426230818_I18N_InitialMigration")]
    partial class I18N_InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("i18n")
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Configuration", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(5)
                        .HasColumnType("nchar(5)")
                        .IsFixedLength();

                    b.Property<string>("BaseUriResource")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Configurations", "i18n");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Language", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(5)
                        .HasColumnType("nchar(5)")
                        .IsFixedLength();

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Languages", "i18n");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Text", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Group")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Texts", "i18n");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Translation", b =>
                {
                    b.Property<string>("LanguageId")
                        .HasColumnType("nchar(5)");

                    b.Property<string>("TextId")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Destination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                    b.HasKey("LanguageId", "TextId");

                    b.HasIndex("TextId");

                    b.ToTable("Translations", "i18n");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Translation", b =>
                {
                    b.HasOne("fbognini.i18n.Persistence.Entities.Language", "Language")
                        .WithMany("Translations")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("fbognini.i18n.Persistence.Entities.Text", "Text")
                        .WithMany("Translations")
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Text");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Language", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("fbognini.i18n.Persistence.Entities.Text", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}