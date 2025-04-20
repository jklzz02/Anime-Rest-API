namespace AnimeApi.Server.Business.Dto;

public record LicensorDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}