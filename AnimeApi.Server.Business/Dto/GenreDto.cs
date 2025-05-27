using AnimeApi.Server.Business.Dto.Interfaces;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record GenreDto : IBaseDto
{
    [JsonProperty( PropertyName = "id")]
    public int? Id { get; init; }
    [JsonProperty( PropertyName = "name")]
    public string? Name { get; init; }
}