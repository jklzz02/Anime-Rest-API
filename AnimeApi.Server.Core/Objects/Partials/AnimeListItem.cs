using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Partials;

public class AnimeListItem
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("title")]
    public string Name { get; set; }
    
    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
}