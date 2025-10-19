
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.SpecHelpers;
public class BaseQuery : QuerySpec<IBaseEntity, BaseQuery>
{
    public BaseQuery ById(int id)
    {
        FilterBy(e => e.Id == id);
        return this;
    }

    public BaseQuery ByName(string name)
    {
        FilterBy(e => EF.Functions.TrigramsAreSimilar(e.Name, name));
        return this;
    }

    public IQuerySpec<TEntity> ToQuerySpec<TEntity>()
        where TEntity : class, IBaseEntity
    {
        return (IQuerySpec<TEntity>) this;
    }
}