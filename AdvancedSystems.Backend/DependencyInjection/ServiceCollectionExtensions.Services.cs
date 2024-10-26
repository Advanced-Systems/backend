using System;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Middlewares;
using AdvancedSystems.Backend.Services;
using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AdvancedSystems.Backend.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
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
    ///     Adds the default implementation of <seealso cref="IInfrastructureService"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAdd(ServiceDescriptor.Scoped<IInfrastructureService, InfrastructureService>());

        return services;
    }
}
