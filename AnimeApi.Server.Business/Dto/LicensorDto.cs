using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record LicensorDto
{
    [JsonProperty("id")]
    public required int? Id { get; init; }
    [JsonProperty("name")]
    public required string? Name { get; init; }
}