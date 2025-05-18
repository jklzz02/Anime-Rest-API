using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record TypeDto
{
    [JsonProperty("id")]
    public int? Id { get; init; }
    [JsonProperty("name")]
    public string? Name { get; init; }
}