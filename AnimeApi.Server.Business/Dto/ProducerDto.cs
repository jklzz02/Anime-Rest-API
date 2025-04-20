namespace AnimeApi.Server.Business.Dto;

public record ProducerDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}