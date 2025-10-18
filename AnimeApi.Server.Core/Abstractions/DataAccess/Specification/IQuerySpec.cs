
using System.Linq.Expressions;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Specification;

public interface IQuerySpec<TEntity> where TEntity : class
{
    IQueryable<TEntity> Apply(IQueryable<TEntity> query);
}