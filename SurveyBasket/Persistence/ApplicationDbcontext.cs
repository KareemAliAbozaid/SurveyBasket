
using System.Reflection;

namespace SurveyBasket.Persistence
{
    public class ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options):DbContext(options)
    {
        public DbSet<Poll>Polls { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
