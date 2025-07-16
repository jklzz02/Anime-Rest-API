using AnimeApi.Server.Core.Abstractions.Dto;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record GenreDto : IBaseDto
{
    [JsonProperty( PropertyName = "id")]
    public int? Id { get; init; }
    [JsonProperty( PropertyName = "name")]
    public string? Name { get; init; }
}