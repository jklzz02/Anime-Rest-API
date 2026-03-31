using AnimeApi.Server.Recommender.Grpc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnimeApi.Server.Handlers.HealthCheck;

public class RecommenderHealthCheck(AnimeRecommenderHealth.AnimeRecommenderHealthClient client) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await
                client
                    .HealthCheckAsync(
                        new HealthCheckRequest(),
                        cancellationToken: cancellationToken);

            var data = new Dictionary<string, object>
            {
                ["version"] = response.Version,
                ["service_status"] = response.ServiceStatus,
                ["anime_loader"] = new
                {
                    is_loaded = response.AnimeLoaderStatus.IsLoaded,
                    has_error = response.AnimeLoaderStatus.HasError,
                    anime_count = response.AnimeLoaderStatus.AnimeCount,
                    error_message = response.AnimeLoaderStatus.ErrorMessage,
                    cache = new
                    {
                        hits = response.AnimeLoaderStatus.CacheHits,
                        misses = response.AnimeLoaderStatus.CacheMisses,
                        size = response.AnimeLoaderStatus.CacheSize,
                        max_size = response.AnimeLoaderStatus.CacheMaxSize,
                    }
                },
                
                ["dataset_status"] = response.DataSetsStatus.SetStatus
                    .Select(s => new { file = s.File, status = s.Status })
                    .ToList()
            };

            if (!response.AnimeLoaderStatus.IsLoaded || response.AnimeLoaderStatus.HasError)
                return HealthCheckResult.Degraded(
                    description: response.AnimeLoaderStatus.ErrorMessage,
                    data: data
                );

            if (!response.DataSetsStatus.IsHealthy)
                return HealthCheckResult.Degraded(
                    description: "One or more datasets are unhealthy",
                    data: data
                );

            return HealthCheckResult.Healthy(
                description: "Recommender service is healthy",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                description: "Recommender service is unreachable",
                exception: ex
            );
        }
    }
}