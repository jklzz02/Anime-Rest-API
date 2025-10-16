
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;
public abstract class Repository<TModel> : IRepository<TModel> where TModel : class
{
    protected AnimeDbContext _context;

    public Repository(AnimeDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountAsync(ISpecification<TModel> specification)
        => await specification.Apply(_context.Set<TModel>()).CountAsync();

    public async Task<TModel?> FindFirstOrDefaultAsync(ISpecification<TModel> specification)
        => await specification.Apply(_context.Set<TModel>()).FirstOrDefaultAsync();

    public async Task<IEnumerable<TModel>> FindAsync(ISpecification<TModel> specification)
     => await specification.Apply(_context.Set<TModel>()).ToListAsync();

    public async Task<IEnumerable<TModel>> GetAllAsync()
     => await _context.Set<TModel>().ToListAsync();

    public abstract Task<Result<TModel>> AddAsync(TModel entity);

    public abstract Task<Result<TModel>> UpdateAsync(TModel entity);

    public abstract Task<bool> DeleteAsync(int id);
}
