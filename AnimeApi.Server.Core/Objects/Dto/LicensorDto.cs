using AnimeApi.Server.Core.Abstractions.Dto;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record LicensorDto : IBaseDto
{
    [JsonProperty("id")]
    public int? Id { get; init; }
    [JsonProperty("name")]
    public string? Name { get; init; }
}