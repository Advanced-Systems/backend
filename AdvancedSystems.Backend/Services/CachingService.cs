using System;
using System.Buffers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;

using Microsoft.Extensions.Caching.Distributed;

namespace AdvancedSystems.Backend.Services;

public sealed class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;

    public CachingService(IDistributedCache distributedCache)
    {
        this._distributedCache = distributedCache;
    }

    #region Public Methods

    public byte[] Serialize<T>(T value) where T : class, new()
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize<T>(writer, value);
        return buffer.WrittenSpan.ToArray();
    }

    public T Deserialize<T>(byte[] values) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(values, nameof(values));
        var payload = new Utf8JsonReader(values);
        return JsonSerializer.Deserialize<T>(ref payload)!;
    }

    public async ValueTask SetAsync<T>(string? key, T value, CancellationToken cancellationToken = default) where T : class, ICacheable, new()
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        byte[] cacheValue = this.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = value?.ExpiryDate,
        };

        await this._distributedCache.SetAsync(key, cacheValue, options, cancellationToken);
    }

    public async ValueTask<T?> GetAsync<T>(string? key, CancellationToken cancellationToken = default) where T : class, ICacheable, new()
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        byte[]? cachedValues = await this._distributedCache.GetAsync(key, cancellationToken);

        if (cachedValues == null || cachedValues.Length == 0) return default;

        T @object = this.Deserialize<T>(cachedValues);
        return @object;
    }

    #endregion
}