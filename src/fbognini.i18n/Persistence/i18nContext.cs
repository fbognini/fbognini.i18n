using fbognini.i18n.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;

namespace fbognini.i18n.Persistence
{
    internal class I18nContext : DbContext
    {
        private readonly ContextSettings settings;

        //protected readonly IConfiguration Configuration;

        public I18nContext(DbContextOptions<I18nContext> options, ContextSettings settings) : base(options)
        {
            this.settings = settings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(settings.Schema);
        }

        internal DbSet<Language> Languages { get; set; }
        internal DbSet<Translation> Translations { get; set; }
        internal DbSet<Configuration> Configurations { get; set; }
        internal DbSet<Text> Texts { get; set; }

        public void DetachAllEntities()
        {
#if NET6
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
