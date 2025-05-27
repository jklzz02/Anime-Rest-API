using AnimeApi.Server.Business.Dto.Interfaces;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record ProducerDto : IBaseDto
{
    [JsonProperty("id")]
    public required int? Id { get; init; }
    [JsonProperty("name")]
    public required string? Name { get; init; }
}