using AnimeApi.Server.Business.Dto.Interfaces;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record TypeDto : IBaseDto
{
    [JsonProperty("id")]
    public int? Id { get; init; }
    [JsonProperty("name")]
    public string? Name { get; init; }
}