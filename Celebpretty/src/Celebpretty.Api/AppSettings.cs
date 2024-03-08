using Celebpretty.Infrastructure.Mongo.Configuration;

namespace Celebpretty.Api;

public class AppSettings
{
    public AppSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public MongoSettings Mongo { get; init; }
}
