using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AnimeApi.Server.RequestModels.Recommender;

public class TargetAnimeCompatibilityRequest : IValidatableObject
{
    [JsonProperty("target_anime_ids"), Required]
    public required IEnumerable<int> TargetAnimeIds { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!TargetAnimeIds.Any())
        {
            yield return new ValidationResult(
                "target_anime_ids are required",
                ["target_anime_ids"]);
        }

        if (TargetAnimeIds.Any(id => id < 1))
        {
            yield return new ValidationResult(
                "target_anime_ids must be positive integers",
                ["target_anime_ids"]);
        }
    }
}