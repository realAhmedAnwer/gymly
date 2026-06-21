using Gymly.Application.Interfaces.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;

namespace Gymly.Infrastructure.Services;

public class RedisCacheService : ICacheService, IDisposable
{
    private readonly IDistributedCache _distributedCache;
    private readonly StackExchange.Redis.IConnectionMultiplexer? _connectionMultiplexer;
    private readonly bool _enabled;

    public RedisCacheService(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        
        var connectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        
        try
        {
            _connectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            _enabled = true;
        }
        catch
        {
            _enabled = false;
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (!_enabled)
            return default;

        try
        {
            var bytes = await _distributedCache.GetAsync(key, cancellationToken);
            if (bytes == null || bytes.Length == 0)
                return default;

            return System.Text.Json.JsonSerializer.Deserialize<T>(bytes);
        }
        catch
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (!_enabled)
            return;

        try
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value);
            await _distributedCache.SetAsync(key, bytes, options, cancellationToken);
        }
        catch
        {
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (!_enabled)
            return;

        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch
        {
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        if (!_enabled || _connectionMultiplexer == null)
            return;

        try
        {
            var endPoints = _connectionMultiplexer.GetEndPoints();
            if (endPoints.Length == 0)
                return;

            var server = _connectionMultiplexer.GetServer(endPoints[0]);
            if (server == null)
                return;

            foreach (var key in server.Keys(pattern: $"{prefix}*")!)
            {
                await _distributedCache.RemoveAsync(key.ToString(), cancellationToken);
            }
        }
        catch
        {
        }
    }

    public void Dispose()
    {
        _connectionMultiplexer?.Dispose();
    }
}
