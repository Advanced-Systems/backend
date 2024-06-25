using System;

namespace AdvancedSystems.Backend.Interfaces;

/// <summary>
///     Defines the cache options for an entry in <see cref="ICachingService"/>.
/// </summary>
public interface ICacheable
{
    /// <summary>
    ///     Gets or sets an absolute expiration date for the cache entry.
    /// </summary>
    public DateTimeOffset? ExpiryDate { get; init; }
}
