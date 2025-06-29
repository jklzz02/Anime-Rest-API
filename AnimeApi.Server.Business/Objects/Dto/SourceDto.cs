using AnimeApi.Server.Business.Objects.Dto.Interfaces;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Objects.Dto;

public record SourceDto : IBaseDto
{
    [JsonProperty("id")]
    public int? Id { get; init; }
    [JsonProperty("name")]
    public string? Name { get; init; }
}