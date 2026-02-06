using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AnimeApi.Server.RequestModels;

public record BanRequest : IValidatableObject
{
    [JsonProperty("email", Required = Required.Always)]
    public required string  Email { get; init; }

    [JsonProperty("expiration")]
    public DateTime? Expiration { get; init; }
    
    [JsonProperty("reason"), MaxLength(250)]
    public string? Reason { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Email))
        {
            yield return new ValidationResult(
                "User email is required", 
                ["email"]);
        }

        if (Expiration.HasValue && Expiration < DateTime.UtcNow)
        {
            yield return new ValidationResult(
                "Ban expiration cannot be set in the past",
                ["expiration"]);
        }

        if (!string.IsNullOrEmpty(Reason) && Reason.Length > 250)
        {
            yield return new ValidationResult(
                "Ban reason cannot be more than 250 characters long",
                ["reason"]);
        }
    }
}