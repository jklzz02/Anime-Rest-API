using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Mappers;

public class BanMapper : Mapper<Ban, BanDto>
{
    public override BanDto MapToDto(Ban entity)
    {
        return new BanDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt,
            Expiration = entity.Expiration,
            NormalizedEmail = entity.NormalizedEmail,
            Reason = entity.Reason,
            Version = entity.Version
        };
    }

    public override Ban MapToEntity(BanDto dto)
    {
        return new Ban
        {
            Id = dto.Id,
            UserId = dto.UserId,
            CreatedAt = dto.CreatedAt,
            Expiration = dto.Expiration,
            NormalizedEmail = dto.NormalizedEmail,
            Reason = dto.Reason,
            Version = dto.Version
        };
    }
}