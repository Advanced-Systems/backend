using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services.HealthChecks;

using Microsoft.Extensions.DependencyInjection;

namespace AdvancedSystems.Backend.Core.Extensions
{
    internal static class AddMethods
    {
        internal static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

            services.AddHealthChecks()
                .AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

            return services;
        }
    }
}
