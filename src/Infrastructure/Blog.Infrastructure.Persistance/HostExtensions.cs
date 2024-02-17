using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Blog.Domain;
using Blog.Domain.Entities;


namespace Blog.Infrastructure.Persistance;

public static class HostExtensions
{
    public static void ConfigurePersistance(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql
            (configuration.GetConnectionString("Postgres")));

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
    }
}
