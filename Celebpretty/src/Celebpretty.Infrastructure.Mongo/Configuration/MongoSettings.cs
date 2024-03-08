namespace Celebpretty.Infrastructure.Mongo.Configuration;

public class MongoSettings
{
    public string ConnectionString { get; init; }
    public string Database { get; init; }
    public string CelebrityCollectionName { get; init; }
    public string SequencesCollectionName { get; init; }
}
