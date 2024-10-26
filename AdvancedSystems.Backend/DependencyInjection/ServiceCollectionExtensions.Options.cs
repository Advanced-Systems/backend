using System;

using AdvancedSystems.Backend.Configuration;
using AdvancedSystems.Backend.Core;
using AdvancedSystems.Backend.Core.Validators;
using AdvancedSystems.Core.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers and binds <seealso cref="AppSettings"/> to the underlying <paramref name="services"/> collection.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <param name="configuration">
    ///     The configuration instance containing the application settings.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Raised if no matching sub-section with the value of <seealso cref="Sections.APP_SETTINGS"/> is found .
    /// </exception>
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddOptions<AppSettings>(configuration.GetRequiredSection(Sections.APP_SETTINGS));
        services.AddSingleton<IValidateOptions<AppSettings>, AppSettingsValidator>();

        return services;
    }
}