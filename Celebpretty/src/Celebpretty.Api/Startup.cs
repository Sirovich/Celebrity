using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Celebpretty.Api.Extensions;
using Celebpretty.Api.Filters;
using Celebpretty.Application.Main.Extensions;
using Celebpretty.Infrastructure.Mongo.Configuration;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Internal;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Celebpretty.Api;

public class Startup
{
    private AppSettings AppSettings { get; }

    public Startup(IConfiguration configuration)
    {
        AppSettings = new AppSettings(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                var jsonFormatter = options.InputFormatters.OfType<SystemTextJsonInputFormatter>().Single();
                jsonFormatter.SupportedMediaTypes.Clear();
                jsonFormatter.SupportedMediaTypes.Add("application/json");
                var outputJsonFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Single();
                outputJsonFormatter.SupportedMediaTypes.Clear();
                outputJsonFormatter.SupportedMediaTypes.Add("application/json");
                outputJsonFormatter.SupportedMediaTypes.Add("application/problem+json");

                options.Filters.Add<ProblemDetailsExceptionFilter>();
            });

        services.AddAutoMapper(configurationExpression => configurationExpression.AddProfile(new Models.MappingProfile()));
        services.AddSwaggerGen();
        services.AddAsyncInitializer<DataAsyncInitializer>();
        services.AddMongoPersistence(AppSettings.Mongo);
        services.AddApplicationMain();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSerilogRequestLogging();
        app.UseCors();
        app.UseRouting();
        app.UseEndpoints(_ => { });
    }
}
