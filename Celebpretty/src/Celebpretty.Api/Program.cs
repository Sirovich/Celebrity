using Celebpretty.Api;
using Celebpretty.Application.Main.Extensions;
using Celebpretty.Infrastructure.Mongo.Configuration;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Celebpretty api is starting...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .AddJsonFile($"appsettings.json", false)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddJsonFile($"serilogsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();

    var appSettings = new AppSettings(builder.Configuration);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(configurationExpression => configurationExpression.AddProfile(new Celebpretty.Api.Models.MappingProfile()));
    builder.Services.AddMongoPersistence(appSettings.Mongo);
    builder.Services.AddApplicationMain();
    builder.Services.AddAsyncInitialization();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();

    app.Run();

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
