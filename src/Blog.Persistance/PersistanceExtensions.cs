using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Blog.Domain;

namespace Blog.Persistance;

public static class PersistanceExtensions
{
    /// <summary>
    /// Configures persistence-related services such as database context and repositories.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The configuration settings.</param>
    public static void ConfigurePersistance(this IServiceCollection services,
        IConfiguration configuration)
    {

        // Configures the application's database context with the specified connection string.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql
            (configuration.GetConnectionString("Postgres")));

        // Registers the PostRepository and CommentRepository services with scoped lifetime.
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
