using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Configures application-related services.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Define the Secret Key from TokenService
            TokenService.SecretKey = configuration["Secrets:SecretKey"]
                ?? throw new ArgumentNullException(TokenService.SecretKey, "Secret Key is not defined in appsettings.");

            // Registers the AutoMapper service
            services.AddAutoMapper(typeof(DomainToDto));

            // Registers the PostService and CommentService services with scoped lifetime.
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}
