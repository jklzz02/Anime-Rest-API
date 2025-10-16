
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity, TDto> : IRepository<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly AnimeDbContext _context;
        protected readonly IMapper<TEntity, TDto> _mapper;

        public BaseRepository(AnimeDbContext context, IMapper<TEntity, TDto> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await
                specification
                    .Apply(_context.Set<TEntity>())
                    .CountAsync();
        }


        public async Task<IEnumerable<TDto>> FindAsync(ISpecification<TEntity> specification)
        {
            var result = await
                specification
                    .Apply(_context.Set<TEntity>())
                    .ToListAsync();

            return _mapper.MapTo(result);
        }

        public async Task<TDto?> FindFirstOrDefaultAsync(ISpecification<TEntity> specification)
        {
            var result = await
                specification
                    .Apply(_context.Set<TEntity>())
                    .FirstOrDefaultAsync();

            return result is null
                ? null
                : _mapper.MapTo(result);
        }
        

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var result = await 
                _context.Set<TEntity>()
                    .ToListAsync();

            return _mapper.MapTo(result);
        }

        public abstract Task<Result<TDto>> AddAsync(TEntity entity);

        public abstract Task<Result<TDto>> UpdateAsync(TEntity entity);

        public abstract Task<bool> DeleteAsync(int id);
    }

    public interface IMapper<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        TDto MapTo(TEntity entity);

        IEnumerable<TDto> MapTo(IEnumerable<TEntity> entities);

        TEntity MapTo(TDto dto);

        IEnumerable<TEntity> MapTo(IEnumerable<TDto> dtos);
    }
}
