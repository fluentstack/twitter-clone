namespace FSH.Framework.Core.Caching;

public interface ICacheService
{
    T GetCache<T>(string key);
    Task<T> GetCacheAsync<T>(string key, CancellationToken token = default);

    void RefreshCache(string key);
    Task RefreshCacheAsync(string key, CancellationToken token = default);

    void RemoveCache(string key);
    Task RemoveCacheAsync(string key, CancellationToken token = default);

    void SetCache<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null);
    Task SetCacheAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken cancellationToken = default);
}
