using fbognini.i18n.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

namespace fbognini.i18n.Persistence
{
    internal class I18nContext : DbContext
    {
        private readonly string schema;

        public I18nContext(DbContextOptions<I18nContext> options, IOptions<I18nSettings> i18noptions) : base(options)
        {
            schema = i18noptions.Value.Schema;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(schema);
        }

        internal DbSet<Language> Languages { get; set; }
        internal DbSet<Translation> Translations { get; set; }
        internal DbSet<Configuration> Configurations { get; set; }
        internal DbSet<Text> Texts { get; set; }

        public void DetachAllEntities()
        {
#if NET6_0_OR_GREATER
            this.ChangeTracker.Clear();
#else
            var entries = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
                entry.State = EntityState.Detached;
#endif


        }
    }
}
