using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.SpecHelpers;

public class BaseQuery<TEntity> : QuerySpec<TEntity, BaseQuery<TEntity>>
    where TEntity : class, IBaseEntity, new()
{
    public BaseQuery<TEntity> ById(int id)
        => FilterBy(e => e.Id == id);

    public BaseQuery<TEntity> ByName(string name)
        => FilterBy(e => EF.Functions.TrigramsAreSimilar(e.Name, name));

    public BaseQuery<TEntity> SortByName()
        => SortBy(e => e.Name);
    
    public BaseQuery<TEntity> TieBreaker()
        => SortBy(b => b.Id);
}