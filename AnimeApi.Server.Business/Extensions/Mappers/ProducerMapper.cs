using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides mapping extensions for converting between <see cref="Producer"/> and <see cref="ProducerDto"/> objects.
/// </summary>
public static class ProducerMapper
{
    public static ProducerDto ToDto(this Producer producer)
    {
        return new ProducerDto
        {
            Id = producer.Id,
            Name = producer.Name
        };
    }
    
    public static Producer ToModel(this ProducerDto producerDto)
    {
        return new Producer
        {
            Id = producerDto.Id ?? 0,
            Name = producerDto.Name
        };
    }

    public static IEnumerable<ProducerDto> ToDto(this IEnumerable<Producer> producers)
    {
        return producers.Select(p => p.ToDto());
    }

    public static IEnumerable<Producer> ToModel(this IEnumerable<ProducerDto> producers)
    {
        return producers.Select(p => p.ToModel());
    }
}