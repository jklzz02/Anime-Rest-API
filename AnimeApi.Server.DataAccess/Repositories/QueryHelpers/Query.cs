using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;

public class Query<TModel>
    where TModel : class
{
    private IQueryable<TModel> _query;
    private object? _lastIncludable;
    private bool _includesApplied = false;

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

    public Query<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> includeSelector)
        where TProperty : class
    {
        var includable = _query.Include(includeSelector);
        _query = includable;
        _lastIncludable = includable;
        _includesApplied = true;
        return this;
    }

    public Query<TModel> Include<TProperty>(IEnumerable<Expression<Func<TModel, TProperty>>> includeSelectors)
        where TProperty : class
    {
        foreach (var includeSelector in includeSelectors)
        {
            var includable = _query.Include(includeSelector);
            _query = includable;
            _lastIncludable = includable;
        }

        _includesApplied = true;
        return this;
    }

    public Query<TModel> ThenInclude<TPreviousProperty, TProperty>(
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeSelector)
        where TPreviousProperty : class
    {
        if (_lastIncludable is IIncludableQueryable<TModel, ICollection<TPreviousProperty>> collectionIncludable)
        {
            var result = collectionIncludable.ThenInclude(thenIncludeSelector);
            _query = result;
            _lastIncludable = result;
        }
        else if (_lastIncludable is IIncludableQueryable<TModel, TPreviousProperty> referenceIncludable)
        {
            var result = referenceIncludable.ThenInclude(thenIncludeSelector);
            _query = result;
            _lastIncludable = result;
        }
        else
        {
            throw new InvalidOperationException("ThenInclude must be called after Include or another ThenInclude.");
        }

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
