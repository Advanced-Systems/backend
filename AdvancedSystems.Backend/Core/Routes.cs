namespace AdvancedSystems.Backend.Core;

public readonly struct Routes
{
    /// <summary>
    ///     Evaluates to a versioned controller endpoint.
    /// </summary>
    /// <seealso cref="Versions"/>
    public const string CONTROLLER = "api/v{version:apiVersion}/[controller]";
}
