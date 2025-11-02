
namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;

public abstract class Mapper<TEntity, TDto> : IMapper<TEntity, TDto>
    where TEntity : class, new()
    where TDto : class, new()
{
    public abstract TDto MapToDto(TEntity entity);

    public abstract TEntity MapToEntity(TDto dto);


    public IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entities)
        => entities.Select(MapToDto);


    public IEnumerable<TEntity> MapToEntity(IEnumerable<TDto> dto)
        => dto.Select(MapToEntity);

    public TSpecific AsSpecific<TSpecific>()
        where TSpecific : class, IMapper<TEntity, TDto>
    {
        return this as TSpecific 
            ?? throw new InvalidOperationException($"Could not cast into '{nameof(TSpecific)}'");
    }
}
