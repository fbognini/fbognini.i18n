using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace fbognini.i18n.Persistence
{
    public class i18nContext : DbContext
    {
        //protected readonly IConfiguration Configuration;

        public i18nContext(DbContextOptions<i18nContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        internal DbSet<Entities.Language> Languages { get; set; }
        internal DbSet<Entities.Translation> Translations { get; set; }
        internal DbSet<Entities.Configuration> Configurations { get; set; }
    }
}
