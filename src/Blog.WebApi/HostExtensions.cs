using Blog.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Blog.WebApi
{
    public static class HostExtensions
    {
        public static void InitializeDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
        }
    }
}
