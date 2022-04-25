using fbognini.i18n.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace fbognini.i18n.Persistence
{
    public class I18nContext : DbContext
    {
        //protected readonly IConfiguration Configuration;

        public I18nContext(DbContextOptions<I18nContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        internal DbSet<Language> Languages { get; set; }
        internal DbSet<Translation> Translations { get; set; }
        internal DbSet<Configuration> Configurations { get; set; }
    }
}
