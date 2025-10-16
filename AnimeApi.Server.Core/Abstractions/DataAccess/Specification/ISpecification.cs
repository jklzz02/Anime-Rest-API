
namespace AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
public interface ISpecification<TModel>
{
    public IQueryable<TModel> Apply(IQueryable<TModel> queryable);
}
