using System.Linq.Expressions;
using System.Reflection;

namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;

public abstract class Mapper<TEntity, TDto> : IMapper<TEntity, TDto>
    where TEntity : class, new()
    where TDto : class, new()
{
    private readonly Dictionary<ProfileKey, LambdaExpression> _profiles = new();
    private readonly Dictionary<Type, Delegate> _compiledProjections = new();

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
        var sourceParam = Expression.Parameter(typeof(TEntity), "e");
        var bindings = new List<MemberBinding>();
        
        var destProps = typeof(TResult).GetProperties();
        foreach (var destProp in destProps)
        {
            if (!destProp.CanWrite || !destProp.CanRead)
            {
                continue;
            }

            var sourceProp = typeof(TEntity).GetProperty(destProp.Name);
            if (sourceProp == null)
            {
                continue;
            }

            Expression valueExpr;
            
            if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
            {
                valueExpr = Expression.Property(sourceParam, sourceProp);
            }
            else if (_profiles.TryGetValue(
                new ProfileKey(sourceProp, destProp.PropertyType),
                out var profile))
            {
                var sourceAccess = Expression.Property(sourceParam, sourceProp);
                valueExpr = Inline(profile, sourceAccess);
            }
            else
            {
                continue;
            }

            bindings.Add(Expression.Bind(destProp, valueExpr));
        }

        var body = Expression.MemberInit(
            Expression.New(typeof(TResult)),
            bindings);

        return Expression.Lambda<Func<TEntity, TResult>>(body, sourceParam);
    }

    public TResult ProjectTo<TResult>(TEntity entity)
        where TResult : class, new()
    {
        var cached = _compiledProjections
            .TryGetValue(typeof(TResult), out var resolver);
        
        if (cached)
        {
            return ((Func<TEntity, TResult>)resolver!)(entity);
        }

        var projection = Projection<TResult>().Compile();
        _compiledProjections[typeof(TResult)] = projection;
        return projection(entity);
    }

    protected void Profile<TSource, TDest>(
        Expression<Func<TEntity, TSource>> selector,
        Expression<Func<TSource, TDest>> projection)
        where TSource : class
        where TDest : class
    {
        if (selector.Body is not MemberExpression { Member: PropertyInfo sourceProp } ||
            sourceProp.DeclaringType != typeof(TEntity) ||
            sourceProp.PropertyType != typeof(TSource))
        {
            throw new ArgumentException(
                "Selector must be a direct property access on the entity");
        }

        _profiles[new ProfileKey(sourceProp, typeof(TDest))] = projection;
        _compiledProjections.Clear();
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

    private readonly record struct ProfileKey(PropertyInfo SourceProp, Type DestType);
}
