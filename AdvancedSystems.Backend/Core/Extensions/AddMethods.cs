using AdvancedSystems.Backend.Models.Settings;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services.HealthChecks;
using AdvancedSystems.Backend.Swagger;

using Asp.Versioning;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedSystems.Backend.Core.Extensions;

internal static class AddMethods
{
    internal static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

        services.AddHealthChecks()
                .AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

        return services;
    }

    internal static IServiceCollection AddBackendSettings(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        services.AddOptions<AppSettings>()
                .Bind(configurationRoot.GetRequiredSection(nameof(AppSettings)))
                .ValidateDataAnnotations()
                .Validate(option =>
                {
                    return option.DefaultApiVersion == 1.0 || option.DefaultApiVersion == 2.0;
                })
                .ValidateOnStart();

        return services;
    }

    internal static IServiceCollection AddBackendDocumentation(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        var settings = configurationRoot.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddApiVersioning(option => {
            option.DefaultApiVersion = new ApiVersion(settings!.DefaultApiVersion);
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.ReportApiVersions = true;
            option.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddMvc().AddApiExplorer();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(option => option.OperationFilter<SwaggerDefaultValues>());

        return services;
    }
}