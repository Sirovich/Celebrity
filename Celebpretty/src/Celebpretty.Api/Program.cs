using Celebpretty.Api;
using Serilog;
using System.Diagnostics;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Celebpretty api is starting...");
try
{
    Prometheus.Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
    {
        { "server", Environment.MachineName.ToLowerInvariant() }
    });

    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    Activity.ForceDefaultIdFormat = true;
    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.AddJsonFile($"appsettings.json", false);
            configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
            configurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            configurationBuilder.AddJsonFile($"serilogsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
            configurationBuilder.AddEnvironmentVariables();
        })
        .UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("app-group", "Celebpretty")
                .Enrich.WithProperty("app", "Celebpretty")
                .Enrich.WithProperty("env", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("v", Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3));
        })
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

    var app = builder.Build();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Celebpretty api shutdown complete");
    Log.CloseAndFlush();
}
