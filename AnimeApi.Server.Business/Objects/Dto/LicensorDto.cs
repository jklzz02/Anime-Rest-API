using AnimeApi.Server.Business.Objects.Dto.Interfaces;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Objects.Dto;

public record LicensorDto : IBaseDto
{
    [JsonProperty("id")]
    public required int? Id { get; init; }
    [JsonProperty("name")]
    public required string? Name { get; init; }
}