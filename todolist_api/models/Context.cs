using Microsoft.EntityFrameworkCore;

namespace todolist_api.models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=todolistapi;Username=postgres;Password=Begemot03");
        }
    }
}