using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects.Dto;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Partials;

public class AnimeListItem : IProjectableFrom<AnimeDto>
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("title")]
    public string Name { get; set; }
    
    [JsonProperty("english_title")]
    public string EnglishName { get; set; }
    
    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
}