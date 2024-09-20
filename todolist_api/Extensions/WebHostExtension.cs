using API.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public class WebHostExtensions
    {
        public static void SeedDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = new DatabaseContext(scope.ServiceProvider.GetRequiredService<DbContextOptions>());

            context.Database.Migrate();
        }
    }
}