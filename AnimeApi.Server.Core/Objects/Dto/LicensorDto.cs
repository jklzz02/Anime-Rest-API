using AnimeApi.Server.Core.Abstractions.Dto;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record LicensorDto : IBaseDto
{
    [JsonProperty("id")]
    public required int? Id { get; init; }
    [JsonProperty("name")]
    public required string? Name { get; init; }
}