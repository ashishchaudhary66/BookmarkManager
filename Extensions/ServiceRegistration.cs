using BookmarkManager.Interfaces;
using BookmarkManager.Repositories;
using BookmarkManager.Services;

namespace BookmarkManager.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITagRepository, TagRepository>();


        return services;
    }
}
