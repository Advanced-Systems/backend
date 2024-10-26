using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Middlewares;
using AdvancedSystems.Backend.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedSystems.Backend.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
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
