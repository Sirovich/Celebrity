namespace Celebpretty.Infrastructure.Mongo.Configuration;

public class MongoSettings
{
    public string ConnectionString { get; init; }
    public string Database { get; init; }
    public string CelebCollectionName { get; init; }
}
