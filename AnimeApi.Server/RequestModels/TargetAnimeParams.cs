using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Extensions;
using Newtonsoft.Json;

namespace AnimeApi.Server.RequestModels;

public record TargetAnimeParams : IValidatableObject
{
    [JsonProperty("target_anime_ids")]
    public List<int> TargetAnimeIds { get; init; } = [];
    
    [JsonProperty("order_by")]
    public string OrderBy { get; init; } = Constants.OrderBy.Fields.Score;
    
    [JsonProperty("sort_order")]
    public string SortOrder { get; init; } = Constants.OrderBy.StringDirections.Descending;

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
        
        if (!Constants.OrderBy.Fields.ValidFields.ContainsIgnoreCase(OrderBy))
        {
            yield return new ValidationResult(
                $"order_by must be one of: {string.Join(", ", Constants.OrderBy.Fields.ValidFields)}",
                ["order_by"]);
        }

        if (!Constants.OrderBy.StringDirections.Directions.ContainsIgnoreCase(SortOrder))
        {
            yield return new ValidationResult(
                $"sort_order must be one of: {string.Join(", ", Constants.OrderBy.StringDirections.Directions)}",
                ["sort_order"]);
        }
    }
}