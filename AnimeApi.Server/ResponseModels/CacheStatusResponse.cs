using AnimeApi.Server.Core;
using Newtonsoft.Json;

namespace AnimeApi.Server.ResponseModels;

public record CacheStatusResponse
{
    [JsonProperty("hits")]
    public long Hits { get; init; }
    
    [JsonProperty("misses")]
    public long Misses { get; init; }
    
    [JsonProperty("hit_ratio")]
    public long HitRatio
        => (Hits + Misses) > 0
            ? Hits * 100 / (Hits + Misses)
            : 0;
    
    [JsonProperty("entries_count")]
    public long EntriesCount { get; init;}
    
    [JsonProperty("eviction_count")]
    public long EvictionCount { get; init; }
    
    [JsonProperty("estimated_unit_size")]
    public long EstimatedUnitSize { get; init; }
    
    [JsonProperty("max_unit_cache_size")]
    public int MaxUnitSize
        => Constants.Cache.CacheSize;
    
    [JsonProperty("capacity_used_percent")]
    public long CapacityUsedPercent
        => EstimatedUnitSize > 0 && MaxUnitSize > 0
            ? EstimatedUnitSize * 100 / MaxUnitSize
            : 0;
    
    [JsonProperty("state")]
    public string State
    {
        get
        {
            if (EntriesCount == 0)
                return "empty";

            if (CapacityUsedPercent >= 90)
                return "under_pressure";

            if (HitRatio < 50)
                return "ineffective";

            return "healthy";
        }
    }

}