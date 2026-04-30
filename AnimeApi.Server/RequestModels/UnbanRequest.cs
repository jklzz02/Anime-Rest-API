using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AnimeApi.Server.RequestModels;

public record UnbanRequest
{
    [Required, EmailAddress, JsonProperty("email")]
    public required string Email { get; init; }
}