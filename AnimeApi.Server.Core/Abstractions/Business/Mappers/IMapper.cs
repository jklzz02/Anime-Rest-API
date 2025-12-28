using System.Linq.Expressions;

namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;

public interface IMapper<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    TDto MapToDto(TEntity entity);

    IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entities);

    TEntity MapToEntity(TDto dto);

    IEnumerable<TEntity> MapToEntity(IEnumerable<TDto> dto);

    TSpecific AsSpecific<TSpecific>()
        where TSpecific : class, IMapper<TEntity, TDto>;

    Expression<Func<TEntity, TResult>> Projection<TResult>()
        where TResult : class, new();

    public TResult ProjectTo<TResult>(TEntity entity)
        where TResult : class, new();
}
