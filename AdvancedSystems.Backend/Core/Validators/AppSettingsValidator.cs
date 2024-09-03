using System.Linq;

using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.Core.Validators;

public sealed class AppSettingsValidator : IValidateOptions<AppSettings>
{
    private readonly double[] _versions = [
        Versions.V1, 
        Versions.V2,
    ];

    private readonly IHostEnvironment _environment;

    public AppSettingsValidator(IHostEnvironment environment)
    {
        this._environment = environment;
    }

    public ValidateOptionsResult Validate(string? name, AppSettings options)
    {
        if (!this._versions.Contains(options.DefaultApiVersion))
        {
            return ValidateOptionsResult.Fail($"Unsupported version number in configuration: {options.DefaultApiVersion} in {this._environment.EnvironmentName} environment.");
        }

        return ValidateOptionsResult.Success;
    }
}