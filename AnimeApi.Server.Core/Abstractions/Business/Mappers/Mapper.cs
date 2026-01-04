
using System.Linq.Expressions;

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

    public virtual Expression<Func<TEntity, TResult>> Projection<TResult>()
        where TResult : class, new()
    {
        var entityType = typeof(TEntity);
        var resultType = typeof(TResult);

        var entityParam = Expression.Parameter(entityType, "e");
        var bindings = new List<MemberBinding>();

        foreach (var resultProp in resultType.GetProperties())
        {
            if (!resultProp.CanWrite)
                continue;

            var entityProp = entityType.GetProperty(resultProp.Name);
            if (entityProp == null)
                continue;

            if (entityProp.PropertyType != resultProp.PropertyType)
                continue;

            var propertyAccess = Expression.Property(entityParam, entityProp);
            bindings.Add(Expression.Bind(resultProp, propertyAccess));
        }

        var body = Expression.MemberInit(
            Expression.New(resultType),
            bindings);

        return Expression.Lambda<Func<TEntity, TResult>>(body, entityParam);
    }

    public TResult ProjectTo<TResult>(TEntity entity)
        where TResult : class, new()
        => Projection<TResult>().Compile().Invoke(entity);
}
