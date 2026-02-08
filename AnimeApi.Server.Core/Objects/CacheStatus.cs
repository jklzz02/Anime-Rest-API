using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

/// <summary>
/// Represents the status of the application's in-memory cache.'
/// </summary>
public record CacheStatus
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
    public CacheState State
    {
        get
        {
            if (EntriesCount == 0)
                return CacheState.Empty;

            if (CapacityUsedPercent >= 90)
                return CacheState.UnderPressure;

            if (HitRatio < 50)
                return CacheState.Ineffective;
            
            return CacheState.Healthy;
        }
    }

    public IReadOnlyDictionary<string, object> ToReport()
        => new Dictionary<string, object>
        {
            { "hits", Hits },
            { "misses", Misses },
            { "hit_ratio", HitRatio },
            { "entries_count", EntriesCount },
            { "estimated_unit_size", EstimatedUnitSize },
            { "max_unit_size", MaxUnitSize },
            { "capacity_used_percent", CapacityUsedPercent },
            {"eviction_count", EvictionCount },
            { "state", State }
        };
}

public enum CacheState
{
    [JsonProperty("empty")]
    Empty,
    
    [JsonProperty("under_pressure")]
    UnderPressure,
    
    [JsonProperty("ineffective")]
    Ineffective,
    
    [JsonProperty("healthy")]
    Healthy
}