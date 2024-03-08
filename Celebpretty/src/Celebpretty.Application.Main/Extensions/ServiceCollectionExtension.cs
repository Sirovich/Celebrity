using Microsoft.Extensions.DependencyInjection;

namespace Celebpretty.Application.Main.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationMain(this IServiceCollection services)
    {
        services.AddTransient<ICelebrityService, CelebrityService>();
        services.AddAutoMapper(c => c.AddProfile<MappingProfile>());

        return services;
    }
}
