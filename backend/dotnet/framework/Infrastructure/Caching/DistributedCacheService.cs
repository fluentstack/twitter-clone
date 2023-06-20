using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FSH.Framework.Infrastructure.Caching
{
    internal class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<DistributedCacheService> _logger;
        private readonly ISerializerService _serializer;

        public DistributedCacheService(IDistributedCache cache, ISerializerService serializer, ILogger<DistributedCacheService> logger) =>
            (_cache, _serializer, _logger) = (cache, serializer, logger);

        public T GetCache<T>(string key) =>
            Get(key) is { } data
                ? Deserialize<T>(data)
                : default!;

        private byte[] Get(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            return _cache.Get(key)!;
        }

        public async Task<T> GetCacheAsync<T>(string key, CancellationToken token = default) =>
            await GetAsync(key, token).ConfigureAwait(true) is { } data
                ? Deserialize<T>(data)
                : default!;

        private async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            byte[]? data = await _cache.GetAsync(key, token)!.ConfigureAwait(true);
            return data!;
        }

        public void RefreshCache(string key)
        {
            _cache.Refresh(key);
        }

        public async Task RefreshCacheAsync(string key, CancellationToken token = default)
        {
            await _cache.RefreshAsync(key, token).ConfigureAwait(true);
            _logger.LogDebug("Cache Refreshed : {key}", key);
        }

        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }

        public async Task RemoveCacheAsync(string key, CancellationToken token = default)
        {
            await _cache.RemoveAsync(key, token).ConfigureAwait(true);
        }

        public void SetCache<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null) =>
            Set(key, Serialize(value), slidingExpiration);

        private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null)
        {
            _cache.Set(key, value, GetOptions(slidingExpiration, absoluteExpiration));
            _logger.LogDebug("Added to Cache : {key}", key);
        }

        public Task SetCacheAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken cancellationToken = default) =>
            SetAsync(key, Serialize(value), slidingExpiration, absoluteExpiration, cancellationToken);

        private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken token = default)
        {
            await _cache.SetAsync(key, value, GetOptions(slidingExpiration, absoluteExpiration), token).ConfigureAwait(true);
            _logger.LogDebug("Added to Cache : {key}", key);
        }

        private byte[] Serialize<T>(T item)
        {
            return Encoding.Default.GetBytes(_serializer.Serialize(item));
        }

        private T Deserialize<T>(byte[] cachedData) =>
            _serializer.Deserialize<T>(Encoding.Default.GetString(cachedData));

        private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
        {
            var options = new DistributedCacheEntryOptions();
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Default expiration time of 10 minutes.
            }

            if (absoluteExpiration.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpiration.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); // Default expiration time of 10 minutes.
            }

            return options;
        }
    }
}
