using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Celebpreety.Infrastructure.IdGenerator;
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
using Microsoft.OpenApi.Models;
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
    public static string Version => typeof(Startup).Assembly.GetName().Version?.ToString(2) ?? "1.0";

    public Startup(IConfiguration configuration)
    {
        AppSettings = new AppSettings(configuration);
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning(apiVersioningOptions =>
        {
            apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningOptions.DefaultApiVersion = new(1, 0);
            apiVersioningOptions.ReportApiVersions = true;
            apiVersioningOptions.ApiVersionReader = ApiVersionReader.Combine(new MediaTypeApiVersionReader("api-version"), new HeaderApiVersionReader("api-version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "VVVV";
            options.SubstituteApiVersionInUrl = true;
        });

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
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                var enumConverter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConverter);
            });

        services.AddFluentValidationAutoValidation(configuration =>
        {
            string CamelCaseNameResolver(Type _, MemberInfo memberInfo, LambdaExpression expression)
            {
                static string GetPropertyName(MemberInfo membrInfo, LambdaExpression expr)
                {
                    if (expr == null) return membrInfo?.Name;
                    var chain = PropertyChain.FromExpression(expr);
                    return chain.Count > 0 ? chain.ToString() : membrInfo?.Name;
                }

                return JsonNamingPolicy.CamelCase.ConvertName(GetPropertyName(memberInfo, expression));
            }

            ValidatorOptions.Global.DisplayNameResolver = CamelCaseNameResolver;
            ValidatorOptions.Global.PropertyNameResolver = CamelCaseNameResolver;
        }
        );
        services.AddValidatorsFromAssemblyContaining<Startup>();
        services.AddFluentValidationRulesToSwagger();
        services.AddAutoMapper(configurationExpression => configurationExpression.AddProfile(new Models.MappingProfile()));
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>(sp =>
            new ConfigureSwaggerOptions(sp.GetRequiredService<IApiVersionDescriptionProvider>()));
        services.AddSwaggerGen();
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetSampler(new ParentBasedSampler(new AlwaysOnSampler()))
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService("Celebpretty"))
                    .AddAspNetCoreInstrumentation()
                    .AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources")
                    .AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Debug);
            });

        services.AddHealthChecks()
            .AddMongoDb(AppSettings.Mongo.ConnectionString, AppSettings.Mongo.Database, "mongodb",
                HealthStatus.Unhealthy, ["readiness"], TimeSpan.FromSeconds(2))
            .ForwardToPrometheus();
        services.AddResponseCompression();
        services.AddAsyncInitializer<DataAsyncInitializer>();
        services.AddMongoPersistence(AppSettings.Mongo);
        services.AddIdGenerator();
        services.AddApplicationMain();
    }

    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        };
        forwardedHeadersOptions.KnownProxies.Clear();
        forwardedHeadersOptions.KnownNetworks.Clear();
        app.UseForwardedHeaders(forwardedHeadersOptions);

        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
                c.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"{description.GroupName}{(description.IsDeprecated ? " (deprecated)" : "")}");

            c.OAuthAppName("Celebpretty Swagger UI");
            c.OAuthUsePkce();
        });

        var currentVersion = provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion).Single();
        var option = new RewriteOptions();
        option.AddRedirect("^$", $"doc-v{currentVersion.GroupName}");
        app.UseRewriter(option);

        app.UseSerilogRequestLogging();
        app.UseCors();
        app.UseRouting();
        app.UseHttpMetrics();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(
                "/health",
                new HealthCheckOptions { Predicate = registration => !registration.Tags.Contains("readiness") }
            );
            endpoints.MapHealthChecks(
                "/readiness",
                new HealthCheckOptions
                {
                    Predicate = registration => registration.Tags.Contains("readiness"),
                }
            );
            endpoints.MapMetrics();
            endpoints.MapControllers().RequireAuthorization();
        });
    }
}
