using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using static AnimeApi.Server.Core.Constants;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;

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

    public TDerived ApplySorting<TKey>(params Expression<Func<TModel, TKey>>[]? orderBy)
        => ApplySorting(Directions.Asc, orderBy);
    
    public TDerived ApplySorting<TKey>(Directions direction, params Expression<Func<TModel, TKey>>[]? orderBy)
    {
        if (orderBy != null && orderBy.Any())
        {
            _query = direction == Directions.Desc
                ? _query.OrderByDescending(orderBy.First())
                : _query.OrderBy(orderBy.First());

            orderBy.Skip(1)
                .ToList()
                .ForEach(ob => _query = direction == Directions.Desc
                    ? ((IOrderedQueryable<TModel>)_query).ThenByDescending(ob)
                    : ((IOrderedQueryable<TModel>)_query).ThenBy(ob));
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
