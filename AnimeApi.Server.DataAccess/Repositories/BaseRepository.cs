
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories
{
    public class BaseRepository<TEntity, TDto> : IRepository<TEntity, TDto>
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
            List<TEntity> result = await
                specification
                    .Apply(_context.Set<TEntity>())
                    .ToListAsync();

            return _mapper.MapToDto(result);
        }

        public async Task<TDto?> FindFirstOrDefaultAsync(ISpecification<TEntity> specification)
        {
            TEntity? result = await
                specification
                    .Apply(_context.Set<TEntity>())
                    .FirstOrDefaultAsync();

            return result is null
                ? null
                : _mapper.MapToDto(result);
        }
        

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            List<TEntity> result = await
                _context.Set<TEntity>()
                    .ToListAsync();

            return _mapper.MapToDto(result);
        }

        public async Task<Result<TDto>> AddAsync(TEntity entity)
        {
           var createdEntity = await 
                _context.Set<TEntity>()
                    .AddAsync(entity);

            bool saveResult = await _context.SaveChangesAsync() > 0;

            return saveResult
                ? Result<TDto>.Success(_mapper.MapToDto(createdEntity.Entity))
                : Result<TDto>.InternalFailure("Failed to add entity.", "An error occurred while saving the entity to the database.");
        }

        public async Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await _context.Set<TEntity>().AddRangeAsync(entity);
            bool saveResult = await _context.SaveChangesAsync() > 0;

            return saveResult
                ? Result<IEnumerable<TDto>>.Success(_mapper.MapToDto(entity))
                : Result<IEnumerable<TDto>>.InternalFailure("Failed to add entities.", "An error occurred while saving the entities to the database.");
        }

        public async Task<Result<TDto>> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            bool saveResult = await _context.SaveChangesAsync() > 0;
            return saveResult
                ? Result<TDto>.Success(_mapper.MapToDto(entity))
                : Result<TDto>.InternalFailure("Failed to update entity.", "An error occurred while saving the entity to the database.");
        }

        public async Task<Result<IEnumerable<TDto>>> UpdateRangeAsync(IEnumerable<TEntity> entity)
        {
            _context.Set<TEntity>().UpdateRange(entity);
            bool saveResult = await _context.SaveChangesAsync() > 0;
            return saveResult
                ? Result<IEnumerable<TDto>>.Success(_mapper.MapToDto(entity))
                : Result<IEnumerable<TDto>>.InternalFailure("Failed to update entities.", "An error occurred while saving the entities to the database.");
        }

        public async Task<bool> DeleteAsync(ISpecification<TEntity> specification)
        {
            TDto? result = await
                FindFirstOrDefaultAsync(specification);

            if (result is null)
            {
                return false;
            }

            _context.Set<TEntity>().Remove(_mapper.MapToEntity(result)!);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(ISpecification<TEntity> specification)
        {
            var result = await
                FindAsync(specification);

            if (!result.Any())
            {
                return false;
            }

            _context.Set<TEntity>().RemoveRange(_mapper.MapToEntity(result)!);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task<bool> ExistsAsync(ISpecification<TEntity> specification)
        {
            return await CountAsync(specification) > 0;
        }
    }

    public interface IMapper<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        TDto MapToDto(TEntity entity);

        IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entities);

        TEntity MapToEntity(TDto dto);

        IEnumerable<TEntity> MapToEntity(IEnumerable<TDto> dtos);
    }
}
