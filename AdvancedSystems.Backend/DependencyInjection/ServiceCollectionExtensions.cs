using System;
using System.Net.Mime;

using AdvancedSystems.Backend.Core;
using AdvancedSystems.Backend.Core.Validators;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Middlewares;
using AdvancedSystems.Backend.Models.Settings;
using AdvancedSystems.Backend.Services.HealthChecks;
using AdvancedSystems.Backend.Swagger;
using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

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

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppSettings>()
                .Bind(configuration.GetRequiredSection(nameof(AppSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddSingleton<IValidateOptions<AppSettings>, AppSettingsValidator>();

        return services;
    }

    public static IServiceCollection AddBackendServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddGlobalExceptionHandler();
        services.AddCachingService(environment);

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

    #region Service Registration

    /// <summary>
    ///     Adds the <seealso cref="GlobalExceptionHandler"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
                ctx.ProblemDetails.Extensions.Remove("exception");
            };
        });

        return services;
    }

    /// <summary>
    ///      Adds a default implementation of <seealso cref="ICachingService"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <param name="environment">
    ///     The hosting environment the web application is running in.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    public static IServiceCollection AddCachingService(this IServiceCollection services, IHostEnvironment environment)
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

        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());

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

    #endregion
}