using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Sorting;
using Newtonsoft.Json;

namespace AnimeApi.Server.RequestModels;

public record TargetAnimeParams : IValidatableObject
{
    [JsonProperty("target_anime_ids")]
    public List<int> TargetAnimeIds { get; init; } = [];
    
    [JsonProperty("order_by")]
    public string OrderBy { get; init; } = SortConstants.Anime.Score;
    
    [JsonProperty("sort_order")]
    public string SortOrder { get; init; } = SortConstants.Descending;

    [JsonProperty("include_adult_content")]
    public bool IncludeAdultContent { get; init; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!TargetAnimeIds.Any())
        {
            yield return new ValidationResult(
                "target_anime_ids are required",
                ["target_anime_ids"]);
        }
        
        if (!AnimeSortMap.Validate(OrderBy))
        {
            yield return new ValidationResult(
                $"order_by must be one of: {string.Join(", ", AnimeSortMap.Fields)}",
                ["order_by"]);
        }

        if (!SortConstants.Directions.ContainsIgnoreCase(SortOrder))
        {
            yield return new ValidationResult(
                $"sort_order must be one of: {string.Join(", ", SortConstants.Directions)}",
                ["sort_order"]);
        }
    }
}