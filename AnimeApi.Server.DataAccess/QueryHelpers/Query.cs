using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.QueryHelpers;

public class Query<TModel, TDerived>
    where TModel : class
    where TDerived : Query<TModel, TDerived>
{
    protected IQueryable<TModel> _query;
    public Query(IQueryable<TModel> query)
    {
        _query = query;
    }

    public TDerived ApplyFilter(Expression<Func<TModel, bool>>? filter)
    {
        if (filter != null)
        {
            _query = _query.Where(filter);
        }

        return (TDerived) this;
    }

    public TDerived ApplyFilters(IEnumerable<Expression<Func<TModel, bool>>>? filters)
    {
        if (filters?.Any() ?? false)
        {
            _query = filters.Aggregate(_query, (current, filter) => current.Where(filter));
        }
        return (TDerived) this;
    }

    public TDerived ApplySorting(Expression<Func<TModel, object?>>? keySelector, SortDirections direction)
    {
        if (keySelector != null)
        {
            _query = direction == SortDirections.Desc
                ? _query.OrderByDescending(keySelector)
                : _query.OrderBy(keySelector);
        }
        return (TDerived) this;
    }

    public TDerived ApplySorting(SortAction<TModel>? sortAction)
    {
        if (sortAction != null)
        {
            _query = sortAction.Direction == SortDirections.Desc
                ? _query.OrderByDescending(sortAction.KeySelector)
                : _query.OrderBy(sortAction.KeySelector);
        }
        return (TDerived) this;
    }
    
    public TDerived ApplySorting(IEnumerable<SortAction<TModel>> sortActions)
    {
        if (sortActions is null || !sortActions.Any())
            return (TDerived)this;

        IOrderedQueryable<TModel>? orderedQuery = null;

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

    public IQueryable<TModel> Build()
        => _query;
}

public class SortAction<TModel>
{
    public Expression<Func<TModel, object?>> KeySelector { get; }
    public SortDirections Direction { get; }

    private SortAction(Expression<Func<TModel, object?>> keySelector, SortDirections direction)
    {
        KeySelector = keySelector;
        Direction = direction;
    }

    public static SortAction<TModel> Asc(Expression<Func<TModel, object?>> keySelector)
        => new(keySelector, SortDirections.Asc);

    public static SortAction<TModel> Desc(Expression<Func<TModel, object?>> keySelector)
        => new(keySelector, SortDirections.Desc);
}

public enum SortDirections
{
    Asc,
    Desc
}