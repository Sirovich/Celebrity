using Celebpretty.Application.Persistence;
using Celebpretty.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Celebpretty.Infrastructure.Scraper.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddScraper(this IServiceCollection services)
    {
        services.AddSingleton<IScraper, Scraper>();
        services.AddAutoMapper(c => c.AddProfile<MappingProfile>());

        return services;
    }
}
