using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Abstractions.Dto;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public class BaseMapper<TEntity, TDto> : IMapper<TEntity, TDto>
    where TEntity : class, IBaseModel, new()
    where TDto : class, IBaseDto, new()
{
    public TDto MapToDto(TEntity entity)
        => new TDto { Id = entity.Id, Name = entity.Name };

    public IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entities)
        => entities.Select(e => MapToDto(e));

    public TEntity MapToEntity(TDto dto)
        => new TEntity { Id = dto.Id ?? default, Name = dto.Name };

    public IEnumerable<TEntity> MapToEntity(IEnumerable<TDto> dto)
        => dto.Select(d => MapToEntity(d));
}