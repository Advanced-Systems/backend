using Microsoft.OpenApi.Models;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using NLog.Extensions.Logging;

namespace AdvancedSystems.Backend.Base;

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
            options.DefaultApiVersion = new ApiVersion(defaultApiVersion);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });

        versioningBuilder.AddApiExplorer(option => {
            option.GroupNameFormat = "'v'VVV";
            option.FormatGroupName = (group, version) => $"{group} ({version})";
            // option.SubstituteApiVersionInUrl = true;
        });

        return versioningBuilder.Services;
    }

    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services, OpenApiInfo openApiInfo)
    {
        return services.AddSwaggerGen(gen => {
            gen.SwaggerDoc(openApiInfo.Version, openApiInfo);
        });
    }
}
