using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddOptions<TOptions>(this IServiceCollection services, IConfigurationSection configurationSection) where TOptions : class
    {
        bool hasOptions = services.Any(service => service.ServiceType == typeof(IConfigureOptions<TOptions>));

        if (!hasOptions)
        {
            services.AddOptions<TOptions>()
                .Bind(configurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        return services;
    }

    public static IServiceCollection TryAddOptions<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class, new()
    {
        services.TryAddSingleton(provider =>
        {
            var options = new TOptions();
            configureOptions(options);
            return Options.Create(options);
        });

        return services;
    }
}
