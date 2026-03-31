using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

public record UserReport
{
    [JsonProperty("id")]
    public int Id { get; init; }
    
    [JsonProperty("user_name")]
    public string UserName { get; init; } = string.Empty;
    
    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;
}