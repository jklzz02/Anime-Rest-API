using System.Linq.Expressions;
using System.Reflection;

namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;

public abstract class Mapper<TEntity, TDto> : IMapper<TEntity, TDto>
    where TEntity : class, new()
    where TDto : class, new()
{
    private readonly Dictionary<ProfileKey, LambdaExpression> _profiles = new();

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

    public Expression<Func<TEntity, TResult>> Projection<TResult>()
        where TResult : class, new()
    {
        var entityParam = Expression.Parameter(typeof(TEntity), "e");
        var bindings = new List<MemberBinding>();
        
        var resultProps = typeof(TResult).GetProperties();
        foreach (var resultProp in resultProps)
        {
            if (!resultProp.CanWrite || !resultProp.CanRead)
            {
                continue;
            }

            var entityProp = typeof(TEntity).GetProperty(resultProp.Name);
            if (entityProp == null)
            {
                continue;
            }

            Expression valueExpr;
            
            if (resultProp.PropertyType.IsAssignableFrom(entityProp.PropertyType))
            {
                valueExpr = Expression.Property(entityParam, entityProp);
            }
            else if (_profiles.TryGetValue(
                new ProfileKey(entityProp.PropertyType, resultProp.PropertyType),
                out var profile))
            {
                var sourceAccess = Expression.Property(entityParam, entityProp);
                valueExpr = Inline(profile, sourceAccess);
            }
            else
            {
                continue;
            }

            bindings.Add(Expression.Bind(resultProp, valueExpr));
        }

        var body = Expression.MemberInit(
            Expression.New(typeof(TResult)),
            bindings);

        return Expression.Lambda<Func<TEntity, TResult>>(body, entityParam);
    }

    public TResult ProjectTo<TResult>(TEntity entity)
        where TResult : class, new()
        => Projection<TResult>().Compile().Invoke(entity);

    protected void Profile<TSource, TDest>(
        Expression<Func<TEntity, TSource>> selector,
        Expression<Func<TSource, TDest>> projection)
        where TSource : class
        where TDest : class
    {
        if (selector.Body is not MemberExpression { Member: PropertyInfo prop } ||
            prop.DeclaringType != typeof(TEntity) ||
            prop.PropertyType != typeof(TSource))
        {
            throw new ArgumentException(
                "Selector must be a direct property access on the entity");
        }

        _profiles[new ProfileKey(typeof(TSource), typeof(TDest))] = projection;
    }

    private static Expression Inline(
        LambdaExpression lambda,
        Expression argument)
        => new ReplaceVisitor(lambda.Parameters[0], argument)
            .Visit(lambda.Body)!;

    private sealed class ReplaceVisitor(ParameterExpression source, Expression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
            => node == source
                ? target
                : base.VisitParameter(node);
    }

    private readonly record struct ProfileKey(Type SourceType, Type TargetType);
}
