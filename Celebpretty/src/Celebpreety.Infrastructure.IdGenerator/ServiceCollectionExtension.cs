using Celebpretty.Application.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Celebpreety.Infrastructure.IdGenerator;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddIdGenerator(this IServiceCollection services)
    {
        return services.AddSingleton<IIdGenerator, IdGenerator>();
    }
}
