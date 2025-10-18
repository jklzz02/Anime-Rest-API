
using System.Linq.Expressions;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Specification;

public interface IQuerySpec<TEntity> where TEntity : class
{
    //IQuerySpec<TEntity> FilterBy(Expression<Func<TEntity, bool>>? filter);
    //IQuerySpec<TEntity> FilterBy(IEnumerable<Expression<Func<TEntity, bool>>>? filters);
    //IQuerySpec<TEntity> SortingBy(Expression<Func<TEntity, object?>>? keySelector, SortDirections direction);
    //IQuerySpec<TEntity> SortBy(SortAction<TEntity>? sortAction);
    //IQuerySpec<TEntity> SortBy(IEnumerable<SortAction<TEntity>> sortActions);
    //IQuerySpec<TEntity> Paginate(int page, int size);
    //IQuerySpec<TEntity> Limit(int size);
    //IQuerySpec<TEntity> AsExpandable();
    //IQuerySpec<TEntity> AsNoTracking();
    //IQuerySpec<TEntity> AsSplitQuery();
    IQueryable<TEntity> Apply(IQueryable<TEntity> query);
}

public class SortAction<TEntity>
{
    public Expression<Func<TEntity, object?>> KeySelector { get; }
    public SortDirections Direction { get; }

    private SortAction(Expression<Func<TEntity, object?>> keySelector, SortDirections direction)
    {
        KeySelector = keySelector;
        Direction = direction;
    }

    public static SortAction<TEntity> Asc(Expression<Func<TEntity, object?>> keySelector)
        => new(keySelector, SortDirections.Asc);

    public static SortAction<TEntity> Desc(Expression<Func<TEntity, object?>> keySelector)
        => new(keySelector, SortDirections.Desc);
}

public enum SortDirections
{
    Asc,
    Desc
}