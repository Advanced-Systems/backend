using System;
using System.Net.Mime;

using AdvancedSystems.Backend.Abstractions.Interfaces;
using AdvancedSystems.Backend.Configuration;
using AdvancedSystems.Backend.Core;
using AdvancedSystems.Backend.Services;
using AdvancedSystems.Core.DependencyInjection;

using Asp.Versioning;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedSystems.Backend.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppSettings(configuration);

        return services;
    }

    public static IServiceCollection AddBackendServices(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            // Should only be used in single server scenarios as this cache stores items in memory and doesn't
            // expand across multiple machines. For those scenarios it is recommended to use a proper distributed
            // cache that can expand across multiple machines.
            services.AddDistributedMemoryCache();
        }
        else
        {
            // See also: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0#distributed-redis-cache
            throw new NotImplementedException("TODO: Enable Redis Caching");
        }

        services.AddCachingService();
        services.AddGlobalExceptionHandler();
        services.AddInfrastructureService();

        return services;
    }

    public static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

        var healthCheckBuilder = services.AddHealthChecks();        
        healthCheckBuilder.AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

        return services;
    }

    public static IServiceCollection AddBackendDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(Versions.DEFAULT);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader(Headers.API_VERSION),
                new UrlSegmentApiVersionReader()
            );
        }).AddMvc().AddApiExplorer();

        services.TryAdd(ServiceDescriptor.Transient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>());
        services.AddSwaggerGen(option => option.OperationFilter<SwaggerDefaultValues>());

        return services;
    }

    /// <summary>
    ///     Adds a health check endpoint for a specified <see cref="IConnectionHealthCheck"/> implementation.
    /// </summary>
    /// <typeparam name="T">
    ///     The type that implements <see cref="IConnectionHealthCheck"/> used to perform the health check.
    /// </typeparam>
    /// <param name="endpoints">
    ///     An <see cref="IEndpointRouteBuilder"/> instance that defines the routing for the health check endpoint.
    /// </param>
    /// <returns>
    ///     An <see cref="IEndpointConventionBuilder"/> that can be used to further configure the health check endpoint.
    /// </returns>
    public static IEndpointConventionBuilder AddHealthCheck<T>(this IEndpointRouteBuilder endpoints) where T : IConnectionHealthCheck
    {
        var healthCheck = endpoints.ServiceProvider.GetRequiredService<T>();

        // TODO: Make constraint type more versatile, see also: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-interfaces 
        return endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
        {
            AllowCachingResponses = true,
            ResponseWriter = async (context, _) =>
            {
                var result = await healthCheck.GetResult();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(result);
            }
        });
    }
}