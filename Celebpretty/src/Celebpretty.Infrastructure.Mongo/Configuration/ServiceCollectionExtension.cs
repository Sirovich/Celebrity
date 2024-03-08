using Celebpretty.Infrastructure.Mongo.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Celebpretty.Infrastructure.Mongo.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings settings)
    {
        services.TryAddSingleton<IMongoClient>(new MongoClient(settings.ConnectionString));
        services.TryAddSingleton(_ =>
            _.GetService<IMongoClient>()
                ?.WithReadPreference(ReadPreference.Primary)
                .WithReadConcern(ReadConcern.Majority)
                .WithWriteConcern(WriteConcern.WMajority)
                .GetDatabase(settings.Database)
                .GetCollection<Celebrity>(settings.CelebCollectionName));

        return services;
    }
}
