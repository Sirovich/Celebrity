using Celebpretty.Application.Persistence;
using Celebpretty.Infrastructure.Common;
using Celebpretty.Infrastructure.Mongo.Models;
using Celebpretty.Infrastructure.Mongo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Celebpretty.Infrastructure.Mongo.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongoPersistence(this IServiceCollection services, MongoSettings settings)
    {
        services.TryAddSingleton<IMongoClient>(new MongoClient(settings.ConnectionString));
        
        services.TryAddSingleton(_ =>
            _.GetService<IMongoClient>()
                ?.WithReadPreference(ReadPreference.Primary)
                .WithReadConcern(ReadConcern.Majority)
                .WithWriteConcern(WriteConcern.WMajority)
                .GetDatabase(settings.Database)
                .GetCollection<Celebrity>(settings.CelebrityCollectionName));

        services.AddSingleton<ICelebrityRepository, CelebrityRepository>();
        services.AddSingleton<IDataInitializer, CelebrityRepository>();

        services.AddAutoMapper(c => c.AddProfile<MappingProfile>());

        return services;
    }
}
