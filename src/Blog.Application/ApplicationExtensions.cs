using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Configures application-related services.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public static void ConfigureApplication(this IServiceCollection services)
        {
            // Registers the AutoMapper service
            services.AddAutoMapper(typeof(DomainToDto));

            // Registers the PostService and CommentService services with scoped lifetime.
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
        }
    }
}
