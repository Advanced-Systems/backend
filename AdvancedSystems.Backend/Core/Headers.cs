namespace AdvancedSystems.Backend.Core;

/// <summary>
///     Configures additional request headers.
/// </summary>
public readonly struct Headers
{
    /// <summary>
    ///     Key used to specify the requested endpoint version in a controller.
    /// </summary>
    /// <seealso cref="Versions"/>
    public const string API_VERSION = "X-API-Version";
}
