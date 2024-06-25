using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedSystems.Backend.Interfaces;

/// <summary>
///     Represents a cache of serialized values.
/// </summary>
public interface ICachingService
{
    /// <summary>
    ///     Serializes an object specified by <typeparamref name="T"/> into UTF-8 encoded bytes.
    /// </summary>
    /// <typeparam name="T">The type argument must be a reference type and  have a public parameterless constructor.</typeparam>
    /// <param name="value"></param>
    /// <returns>An array of bytes representing <paramref name="value"/>.</returns>
    /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.</exception>
    byte[] Serialize<T>(T value) where T : class, new();

    /// <summary>
    ///     Parses the UTF-8 decoded bytes representing a single JSON object into an instance specified by <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type argument must be a reference type and  have a public parameterless constructor.</typeparam>
    /// <param name="values">The bytes representing an instance of <typeparamref name="T"/>.</param>
    /// <returns>The object parsed from <paramref name="values"/>.</returns>
    /// <exception cref="ArgumentNullException">The parameter <paramref name="values"/> is <c>null</c>.</exception>
    /// <exception cref="JsonException">The JSON is invalid.</exception>
    /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.</exception>
    T Deserialize<T>(byte[] values) where T : class, new();

    /// <summary>
    ///     Sets a values in the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">The type argument must be a reference type and  have a public parameterless constructor.</typeparam>
    /// <param name="key">A string identifying the requested values.</param>
    /// <param name="value">The values to set in the cache.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">The parameter <paramref name="key"/> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    ValueTask SetAsync<T>(string? key, T value, CancellationToken cancellationToken = default) where T : class, ICacheable, new();

    /// <summary>
    ///     Gets a values from the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">The type argument must be a reference type and  have a public parameterless constructor.</typeparam>
    /// <param name="key">A string identifying the requested values.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A ValueTask containing the result of type <typeparamref name="T"/> representing the asynchronous operation. The result is null if <paramref name="key"/> can not be identified in the cache.</returns>
    /// <exception cref="ArgumentNullException">The parameter <paramref name="key"/> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    ValueTask<T?> GetAsync<T>(string? key, CancellationToken cancellationToken = default) where T : class, ICacheable, new();
}