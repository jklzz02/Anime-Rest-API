namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;

public interface IMapper<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    TDto MapToDto(TEntity entity);

    IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entities);

    TEntity MapToEntity(TDto dto);

    IEnumerable<TEntity> MapToEntity(IEnumerable<TDto> dtos);

    TSpecific AsSpecific<TSpecific>()
        where TSpecific : class, IMapper<TEntity, TDto>;
}
