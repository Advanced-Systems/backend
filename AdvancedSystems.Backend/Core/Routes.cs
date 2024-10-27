namespace AdvancedSystems.Backend.Core;

internal static class Routes
{
    /// <summary>
    ///     Evaluates to a versioned controller endpoint.
    /// </summary>
    /// <seealso cref="Versions"/>
    internal const string CONTROLLER = "api/v{version:apiVersion}/[controller]";
}