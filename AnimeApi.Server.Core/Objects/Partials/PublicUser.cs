using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Partials;

public record PublicUser
{
    [JsonProperty("id")]
    public int Id { get; init; }
    
    [JsonProperty("username")]
    public string Username { get; init; } =  string.Empty;
    
    [JsonProperty("picture_url")]
    public string PictureUrl { get; init; } =  string.Empty;
}