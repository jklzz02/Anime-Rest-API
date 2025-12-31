using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Abstractions.Dto;

namespace AnimeApi.Server.Core.Mappers;

public class BaseMapper<TEntity, TDto> : Mapper<TEntity, TDto>
    where TEntity : class, IBaseEntity, new()
    where TDto : class, IBaseDto, new()
{
    public override TDto MapToDto(TEntity entity)
        => new TDto { Id = entity.Id, Name = entity.Name };

    public override TEntity MapToEntity(TDto dto)
        => new TEntity { Id = dto.Id ?? default, Name = dto.Name };
}