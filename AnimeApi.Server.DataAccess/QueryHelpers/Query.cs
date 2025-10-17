using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.QueryHelpers;

public abstract class Query<TEntity, TDerived>
    where TEntity : class
    where TDerived : Query<TEntity, TDerived>
{
    protected IQueryable<TEntity> _query;
    public Query(IQueryable<TEntity> query)
    {
        _query = query;
    }

    public TDerived ApplyFilter(Expression<Func<TEntity, bool>>? filter)
    {
        if (filter != null)
        {
            _query = _query.Where(filter);
        }

        return (TDerived) this;
    }

    public TDerived ApplyFilters(IEnumerable<Expression<Func<TEntity, bool>>>? filters)
    {
        if (filters?.Any() ?? false)
        {
            _query = filters.Aggregate(_query, (current, filter) => current.Where(filter));
        }
        return (TDerived) this;
    }

    public TDerived ApplySorting(Expression<Func<TEntity, object?>>? keySelector, SortDirections direction)
    {
        if (keySelector != null)
        {
            _query = direction == SortDirections.Desc
                ? _query.OrderByDescending(keySelector)
                : _query.OrderBy(keySelector);
        }
        return (TDerived) this;
    }

    public TDerived ApplySorting(SortAction<TEntity>? sortAction)
    {
        if (sortAction != null)
        {
            _query = sortAction.Direction == SortDirections.Desc
                ? _query.OrderByDescending(sortAction.KeySelector)
                : _query.OrderBy(sortAction.KeySelector);
        }
        return (TDerived) this;
    }
    
    public TDerived ApplySorting(IEnumerable<SortAction<TEntity>> sortActions)
    {
        if (sortActions is null || !sortActions.Any())
            return (TDerived)this;

        IOrderedQueryable<TEntity>? orderedQuery = null;

        foreach (var sortAction in sortActions)
        {
            if (orderedQuery == null)
            {
                orderedQuery = sortAction.Direction == SortDirections.Desc
                    ? _query.OrderByDescending(sortAction.KeySelector)
                    : _query.OrderBy(sortAction.KeySelector);
            }
            else
            {
                orderedQuery = sortAction.Direction == SortDirections.Desc
                    ? orderedQuery.ThenByDescending(sortAction.KeySelector)
                    : orderedQuery.ThenBy(sortAction.KeySelector);
            }
        }

        if (orderedQuery != null)
        {
            _query = orderedQuery;
        }

        return (TDerived) this;
    }

    public TDerived ApplyPagination(int page, int size)
    {
        _query = _query
            .Skip((page - 1) * size)
            .Take(size);

        return (TDerived) this;
    }

    public TDerived Limit(int size)
    {
        if (size <= 0)
            throw new InvalidOperationException("Size must be greater than 0.");

        _query = _query.Take(size);
        return (TDerived) this;
    }

    public TDerived AsExpandable()
    {
        _query = _query.AsExpandableEFCore();
        return (TDerived) this;
    }

    public TDerived AsNoTracking()
    {
        _query = _query.AsNoTracking();
        return (TDerived) this;
    }

    public TDerived AsSplitQuery()
    {
        _query = _query.AsSplitQuery();
        return (TDerived) this;
    }

    public IQueryable<TEntity> Build()
        => _query;
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

public interface IQuery<TEntity> where TEntity : class
{
    IQuery<TEntity> ApplyFilter(Expression<Func<TEntity, bool>>? filter);

    IQuery<TEntity> ApplyFilters(IEnumerable<Expression<Func<TEntity, bool>>>? filters);

    IQuery<TEntity> ApplySorting(Expression<Func<TEntity, object?>>? keySelector, SortDirections direction);

    IQuery<TEntity> ApplySorting(SortAction<TEntity>? sortAction);

    IQuery<TEntity> ApplySorting(IEnumerable<SortAction<TEntity>> sortActions);

    IQuery<TEntity> ApplyPagination(int page, int size);

    IQuery<TEntity> Limit(int size);

    IQuery<TEntity> AsExpandable();

    IQuery<TEntity> AsNoTracking();

    IQuery<TEntity> AsSplitQuery();

    IQueryable<TEntity> Build();
}