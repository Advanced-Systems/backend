using Microsoft.Extensions.Configuration.Json;

namespace AdvancedSystems.Backend.Base;

public static class Extensions
{
    #region Startup

    public static IConfigurationBuilder AddCustomJsonFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        if (string.IsNullOrEmpty(path)) throw new ArgumentException("The path parameter must be a non-empty string");

        var source = new JsonConfigurationSource
        {
            FileProvider = null,
            Path = path,
            Optional = optional,
            ReloadOnChange = reloadOnChange,
        };

        source.EnsureDefaults(builder);
        source.ResolveFileProvider();
        builder.Add(source);

        return builder;
    }

    #endregion
}
