using System.Reflection;

using Asp.Versioning;
using NLog.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;

namespace AdvancedSystems.Backend.Configuration;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        return services.AddLogging(service =>
        {
            service.ClearProviders();
            service.AddNLog();
        });
    }

    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, int defaultApiVersion)
    {
        var versioningBuilder = services.AddApiVersioning(options => {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(defaultApiVersion);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        });

        versioningBuilder.AddApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.FormatGroupName = (group, version) => $"{group} ({version})";
        });

        return versioningBuilder.Services;
    }

    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services, OpenApiInfo openApiInfo)
    {
        return services.AddSwaggerGen(options => {
            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in service.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo{
                    Title = openApiInfo.Title,
                    Version = description.ApiVersion.ToString(),
                });
            }

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}
