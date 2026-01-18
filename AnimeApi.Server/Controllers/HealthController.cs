using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : Controller
{
    private readonly ICachingService _cache;
    
    public HealthController(ICachingService cache)
    {
        _cache = cache;
    }
    
    [HttpGet("cache")]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CacheStatus()
    {
        var status = _cache.Statistics;
        
        return Ok( new CacheStatusResponse
        {
            Hits = status.TotalHits,
            Misses = status.TotalMisses,
            EstimatedUnitSize = status.CurrentEstimatedSize.GetValueOrDefault(),
            EntriesCount = status.CurrentEntryCount,
            EvictionCount = _cache.EvictionCount
        });
    }
}