using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

public record FacebookResponse
{
    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;

    [JsonProperty("picture")]
    public FacebookPicture Picture { get; init; } = new();
}

public record FacebookPicture
{
    [JsonProperty("data")]
    public FacebookPictureData Data { get; init; } = new();
}

public record FacebookPictureData
{
    [JsonProperty("url")]
    public string Url { get; init; } = string.Empty;
    
    [JsonProperty("id")]
    public string Id { get; init; } = string.Empty;
}
