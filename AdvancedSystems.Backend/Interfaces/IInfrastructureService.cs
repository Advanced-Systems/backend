using Asp.Versioning;

namespace AdvancedSystems.Backend.Interfaces;

/// <summary>
///     Provides internal utility functions for performing commonly used tasks.
/// </summary>
/// <remarks>
///     This service should not be exposed to external consumers.
/// </remarks>
public interface IInfrastructureService
{
    #region Properties

    /// <summary>
    ///     Gets the API version for the current request.
    /// </summary>
    /// <value>
    ///     <inheritdoc cref="IApiVersioningFeature.RequestedApiVersion" path="/value"/>
    /// </value>
    /// <remarks>
    ///     <inheritdoc cref="IApiVersioningFeature.RequestedApiVersion" path="/remarks"/>
    /// </remarks>
    ApiVersion? RequestedApiVersion { get; }

    #endregion
}