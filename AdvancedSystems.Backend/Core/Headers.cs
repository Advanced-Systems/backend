namespace AdvancedSystems.Backend.Core;

/// <summary>
///     Configures additional request headers.
/// </summary>
internal static class Headers
{
    /// <summary>
    ///     Key used to specify the requested endpoint version in a controller.
    /// </summary>
    /// <seealso cref="Versions"/>
    internal const string API_VERSION = "X-API-Version";
}