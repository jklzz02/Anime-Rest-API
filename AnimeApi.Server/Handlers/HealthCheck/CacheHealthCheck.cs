using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnimeApi.Server.Handlers.HealthCheck;

public class CacheHealthCheck(ICachingService cache) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var stats = cache.GetStatistics();

            return stats.State switch
            {
                CacheState.Healthy or CacheState.Empty
                    => Task.FromResult(
                        HealthCheckResult.Healthy(
                            "Cache is healthy",
                            stats.ToReport())),

                CacheState.UnderPressure
                    => Task.FromResult(
                        HealthCheckResult.Degraded(
                            "Cache is under memory pressure",
                            null,
                            stats.ToReport())),

                CacheState.Ineffective
                    => Task.FromResult(
                        HealthCheckResult.Degraded(
                            "Cache hit ratio is low",
                            null,
                            stats.ToReport())),

                _ => Task.FromResult(
                    HealthCheckResult.Unhealthy(
                        "Unknown cache state"))
            };
        }
        catch (Exception ex)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "Cache health check failed",
                    ex));
        }
    }
}
