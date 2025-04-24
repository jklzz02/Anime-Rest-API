using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public class GenreDto
{
    [JsonProperty( PropertyName = "id")]
    public required int Id { get; init; }
    [JsonProperty( PropertyName = "name")]
    public required string Name { get; init; }
}