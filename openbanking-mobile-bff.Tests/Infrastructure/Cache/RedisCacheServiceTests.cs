using Microsoft.Extensions.Caching.Distributed;
using openbanking_mobile_bff.Infrastructure.Cache;

namespace openbanking_mobile_bff.Tests.Infrastructure.Cache;

public sealed class RedisCacheServiceTests
{
    [Fact]
    public async Task GetAsync_WithMissingKey_ReturnsDefault()
    {
        var cache = new FakeDistributedCache();
        var service = new RedisCacheService(cache);

        var result = await service.GetAsync<SampleCacheItem>("missing-key");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_ThenGetAsync_RoundTripsSerializedValue()
    {
        var cache = new FakeDistributedCache();
        var service = new RedisCacheService(cache);
        var value = new SampleCacheItem { Id = "item-1", Count = 3 };

        await service.SetAsync("item-key", value);
        var result = await service.GetAsync<SampleCacheItem>("item-key");

        Assert.NotNull(result);
        Assert.Equal("item-1", result!.Id);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task SetAsync_WithExpiration_SetsAbsoluteExpirationRelativeToNow()
    {
        var cache = new FakeDistributedCache();
        var service = new RedisCacheService(cache);
        var expiration = TimeSpan.FromMinutes(5);

        await service.SetAsync("expiring-key", new SampleCacheItem(), expiration);

        Assert.NotNull(cache.LastSetOptions);
        Assert.Equal(expiration, cache.LastSetOptions!.AbsoluteExpirationRelativeToNow);
    }

    [Fact]
    public async Task RemoveAsync_AfterSet_RemovesValueFromCache()
    {
        var cache = new FakeDistributedCache();
        var service = new RedisCacheService(cache);

        await service.SetAsync("remove-key", new SampleCacheItem { Id = "to-remove", Count = 1 });
        await service.RemoveAsync("remove-key");
        var result = await service.GetAsync<SampleCacheItem>("remove-key");

        Assert.Null(result);
    }

    private sealed class SampleCacheItem
    {
        public string? Id { get; set; }
        public int Count { get; set; }
    }

    private sealed class FakeDistributedCache : IDistributedCache
    {
        private readonly Dictionary<string, string> _storage = new();

        public DistributedCacheEntryOptions? LastSetOptions { get; private set; }

        public byte[]? Get(string key) =>
            _storage.TryGetValue(key, out var value) ? System.Text.Encoding.UTF8.GetBytes(value) : null;

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default) =>
            Task.FromResult(Get(key));

        public void Refresh(string key)
        {
        }

        public Task RefreshAsync(string key, CancellationToken token = default) =>
            Task.CompletedTask;

        public void Remove(string key) =>
            _storage.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            LastSetOptions = options;
            _storage[key] = System.Text.Encoding.UTF8.GetString(value);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Set(key, value, options);
            return Task.CompletedTask;
        }
    }
}
