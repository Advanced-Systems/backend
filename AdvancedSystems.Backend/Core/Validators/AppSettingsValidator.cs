using System.Linq;

using AdvancedSystems.Backend.Models.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.Core.Validators;

public sealed class AppSettingsValidator : IValidateOptions<AppSettings>
{
    private static double[] _versions = new[] { 1.0, 2.0 };

    private readonly IHostEnvironment _environment;

    public AppSettingsValidator(IHostEnvironment environment)
    {
        this._environment = environment;
    }

    public ValidateOptionsResult Validate(string? name, AppSettings options)
    {
        if (!_versions.Contains(options.DefaultApiVersion))
        {
            return ValidateOptionsResult.Fail($"Unsupported version number in configuration: {options.DefaultApiVersion} in {this._environment.EnvironmentName} environment.");
        }

        return ValidateOptionsResult.Success;
    }
}