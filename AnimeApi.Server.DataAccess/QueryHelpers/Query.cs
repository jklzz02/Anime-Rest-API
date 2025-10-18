using System.Linq.Expressions;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.QueryHelpers;

public abstract class QuerySpec<TEntity, TDerived> : IQuerySpec<TEntity>
    where TEntity : class
    where TDerived : QuerySpec<TEntity, TDerived>
{
    private readonly List<Expression<Func<TEntity, bool>>> _filters = new();
    private readonly List<SortAction<TEntity>> _sortActions = new();
    private readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> _includes = new();
    private int? _skip;
    private int? _take;
    private bool _asExpandable;
    private bool _asNoTracking;
    private bool _asSplitQuery;

    public TDerived AsNoTracking()
    {
        _asNoTracking = true;
        return (TDerived)this;
    }

    public TDerived AsSplitQuery()
    {
        _asSplitQuery = true;
        return (TDerived)this;
    }

    public TDerived Paginate(int page, int size)
    {
        _skip = (page - 1) * size;
        _take = size;
        return (TDerived)this;
    }

    public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
    {
        if (_asExpandable)
        {
            query = query.AsExpandableEFCore();
        }

        foreach (var filter in _filters)
        {
            query = query.Where(filter);
        }

        if (_sortActions.Any())
        {
            IOrderedQueryable<TEntity>? orderedQuery = null;

            foreach (var sortAction in _sortActions)
            {
                if (orderedQuery == null)
                {
                    orderedQuery = sortAction.Direction == SortDirections.Desc
                        ? query.OrderByDescending(sortAction.KeySelector)
                        : query.OrderBy(sortAction.KeySelector);
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
                query = orderedQuery;
            }
        }

        if (_skip.HasValue)
        {
            query = query.Skip(_skip.Value);
        }

        if (_take.HasValue)
        {
            query = query.Take(_take.Value);
        }

        if (_asNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (_asSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return query;
    }


    protected TDerived FilterBy(Expression<Func<TEntity, bool>>? filter)
    {
        if (filter != null)
        {
            _filters.Add(filter);
        }
        return (TDerived)this;
    }

    protected TDerived FilterBy(IEnumerable<Expression<Func<TEntity, bool>>>? filters)
    {
        if (filters?.Any() ?? false)
        {
            _filters.AddRange(filters);
        }
        return (TDerived)this;
    }

    protected TDerived SortBy(Expression<Func<TEntity, object?>>? keySelector, SortDirections direction)
    {
        if (keySelector != null)
        {
            _sortActions.Add(direction == SortDirections.Desc
                ? SortAction<TEntity>.Desc(keySelector)
                : SortAction<TEntity>.Asc(keySelector));
        }
        return (TDerived)this;
    }

    protected TDerived SortBy(SortAction<TEntity>? sortAction)
    {
        if (sortAction != null)
        {
            _sortActions.Add(sortAction);
        }
        return (TDerived)this;
    }

    protected TDerived SortBy(IEnumerable<SortAction<TEntity>> sortActions)
    {
        if (sortActions?.Any() ?? false)
        {
            _sortActions.AddRange(sortActions);
        }
        return (TDerived)this;
    }

    protected TDerived Limit(int size)
    {
        if (size <= 0)
            throw new InvalidOperationException("Size must be greater than 0.");

        _take = size;
        return (TDerived)this;
    }

    public TDerived AsExpandable()
    {
        _asExpandable = true;
        return (TDerived)this;
    }

    protected TDerived Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
    {
        _includes.Add(include);
        return (TDerived)this;
    }
}