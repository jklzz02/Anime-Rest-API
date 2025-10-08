using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;

public class Query<TModel>
    where TModel : class
{
    protected IQueryable<TModel> _query;
    public Query(IQueryable<TModel> query)
    {
        _query = query;
    }

    public Query<TModel> ApplyFilter(Expression<Func<TModel, bool>>? filter)
    {
        if (filter != null)
        {
            _query = _query.Where(filter);
        }

        return this;
    }

    public Query<TModel> ApplyFilters(IEnumerable<Expression<Func<TModel, bool>>>? filters)
    {
        if (filters?.Any() ?? false)
        {
            _query = filters.Aggregate(_query, (current, filter) => current.Where(filter));
        }
        return this;
    }

    public Query<TModel> ApplySorting(Expression<Func<TModel, object>>? orderBy)
        => ApplySorting(orderBy, false);

    public Query<TModel> ApplySorting(Expression<Func<TModel, object>>? orderBy, bool desc)
    {
        if (orderBy != null)
        {
            _query = desc
                ? _query.OrderByDescending(orderBy)
                : _query.OrderBy(orderBy);
        }

        return this;
    }

    public Query<TModel> ApplyPagination(int page, int size)
    {
        _query = _query
            .Skip((page - 1) * size)
            .Take(size);

        return this;
    }
    
    public Query<TModel> AsExpandable()
    {
        _query = _query.AsExpandableEFCore();
        return this;
    }

    public Query<TModel> AsNoTracking()
    {
        _query = _query.AsNoTracking();
        return this;
    }

    public Query<TModel> AsSplitQuery()
    {
        _query = _query.AsSplitQuery();
        return this;
    }

    public IQueryable<TModel> Build()
        => _query;
}
