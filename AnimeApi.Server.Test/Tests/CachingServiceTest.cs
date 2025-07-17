using AnimeApi.Server.Business.Services;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeApi.Server.Test.Tests;

public class CachingServiceTest
{
    private IMemoryCache Cache => new MemoryCache(new MemoryCacheOptions());
    
    [Theory]
    [InlineData("1", null)]
    [InlineData("2", "2")]
    [InlineData("3", 3)]
    [InlineData(4, "4")]
    public async Task GetOrCreate_Should_Return_Correct_Cached_Value(object key, object value)
    {
        var service = new CachingService(Cache);
        var result = await service.GetOrCreateAsync(key, () => Task.FromResult(value));
        Assert.Equal(value, result);
    }
    
    [Fact]
    public async Task GetOrCreate_With_Custom_Expiration_Should_Not_Throw()
    {
        var service = new CachingService(Cache);
        var key = "key";

        var result = await service.
            GetOrCreateAsync(key, () => Task.FromResult("temp"), 0, TimeSpan.FromMinutes(1));

        Assert.Equal("temp", result);
        Assert.True(service.HasKey(key));
    }

    [Fact]
    public async Task GetOrCreate_With_Custom_Expiration_Should_Expire_In_Time()
    {
        var service = new CachingService(Cache);
        var key = "key"; 
        await service.
            GetOrCreateAsync(key, () => Task.FromResult("temp"), 0, TimeSpan.FromMicroseconds(1));

        Thread.Sleep(TimeSpan.FromMicroseconds(1));
        Assert.False(service.HasKey(key));
    }

}