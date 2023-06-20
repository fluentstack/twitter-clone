using FSH.Framework.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Caching;

public class InMemoryCacheService : ICacheService
{
    private readonly ILogger<InMemoryCacheService> _logger;
    private readonly IMemoryCache _cache;
    private readonly CachingOptions _cacheOptions;
    public InMemoryCacheService(IMemoryCache cache, ILogger<InMemoryCacheService> logger, IOptions<CachingOptions> cacheOptions)
    {
        _cache = cache;
        _logger = logger;
        _cacheOptions = cacheOptions.Value;
    }

    public T GetCache<T>(string key) => _cache.Get<T>(key)!;

    public Task<T> GetCacheAsync<T>(string key, CancellationToken token = default)
    {
        var data = GetCache<T>(key)!;
        if (data != null)
        {
            _logger.LogDebug("Get From Cache : {Key}", key);
        }
        else
        {
            _logger.LogDebug("Key Not Found in Cache : {Key}", key);
        }
        return Task.FromResult(data);
    }

    public void RefreshCache(string key) => _cache.TryGetValue(key, out _);

    public Task RefreshCacheAsync(string key, CancellationToken token = default)
    {
        RefreshCache(key);
        return Task.CompletedTask;
    }

    public void RemoveCache(string key) => _cache.Remove(key);

    public Task RemoveCacheAsync(string key, CancellationToken token = default)
    {
        RemoveCache(key);
        return Task.CompletedTask;
    }

    public void SetCache<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null)
    {
        slidingExpiration ??= TimeSpan.FromMinutes(_cacheOptions.SlidingExpirationInMinutes);
        absoluteExpiration ??= DateTime.UtcNow.AddMinutes(_cacheOptions.AbsoluteExpirationInMinutes);
        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration, AbsoluteExpiration = absoluteExpiration });
        _logger.LogDebug("Added to Cache : {Key}", key);
    }

    public Task SetCacheAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken token = default)
    {
        SetCache(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}