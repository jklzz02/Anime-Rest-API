using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories
{
    public class Repository<TEntity, TDto> : IRepository<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected AnimeDbContext Context { get; }
        protected IMapper<TEntity, TDto> Mapper { get; set; }

        public Repository(AnimeDbContext context, IMapper<TEntity, TDto> mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(IQuerySpec<TEntity> specification)
        {
            return await specification
                .Apply(Context.Set<TEntity>())
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDto>> FindAsync(IQuerySpec<TEntity> specification)
        {
            var entities = await specification
                .Apply(Context.Set<TEntity>())
                .ToListAsync();

            return Mapper.MapToDto(entities);
        }

        /// <inheritdoc />
        public async Task<TDto?> FindFirstOrDefaultAsync(IQuerySpec<TEntity> specification)
        {
            var entity = await specification
                .Apply(Context.Set<TEntity>())
                .FirstOrDefaultAsync();

            return entity is null ? null : Mapper.MapToDto(entity);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await Context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            return Mapper.MapToDto(entities);
        }

        /// <inheritdoc />
        public virtual async Task<Result<TDto>> AddAsync(TDto dto)
        {
            try
            {
                var entity = Mapper.MapToEntity(dto);
                var createdEntity = await Context.Set<TEntity>().AddAsync(entity);
                
                await Context.SaveChangesAsync();
                
                return Result<TDto>.Success(Mapper.MapToDto(createdEntity.Entity));
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Result<TDto>.InternalFailure(
                    "Concurrency failure while attempting to add entity.",
                    e.Message);
            }
            catch (DbUpdateException e)
            {
                return Result<TDto>.InternalFailure(
                    "Failed to add entity.",
                    e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc />
        public virtual async Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> dto)
        {
            try
            {
                var entities = Mapper.MapToEntity(dto).ToList();
                
                await Context.Set<TEntity>().AddRangeAsync(entities);
                await Context.SaveChangesAsync();
                
                return Result<IEnumerable<TDto>>.Success(Mapper.MapToDto(entities));
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Result<IEnumerable<TDto>>.InternalFailure(
                    "Concurrency failure occurred while adding entities.",
                    e.Message);
            }
            catch (DbUpdateException e)
            {
                return Result<IEnumerable<TDto>>.InternalFailure(
                    "An error occurred while adding entities.",
                    e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc />
        public virtual async Task<Result<TDto>> UpdateAsync(TDto dto)
        {
            try
            {
                var entity = Mapper.MapToEntity(dto);
                
                Context.Set<TEntity>().Update(entity);
                await Context.SaveChangesAsync();
                
                return Result<TDto>.Success(Mapper.MapToDto(entity));
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Result<TDto>.InternalFailure(
                    "Concurrency failure while attempting to update entity.",
                    e.Message);
            }
            catch (DbUpdateException e)
            {
                return Result<TDto>.InternalFailure(
                    "Failed to update entity.",
                    e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc />
        public virtual async Task<Result<IEnumerable<TDto>>> UpdateRangeAsync(IEnumerable<TDto> dto)
        {
            try
            {
                var entities = Mapper.MapToEntity(dto).ToList();
                
                Context.Set<TEntity>().UpdateRange(entities);
                await Context.SaveChangesAsync();
                
                return Result<IEnumerable<TDto>>.Success(Mapper.MapToDto(entities));
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Result<IEnumerable<TDto>>.InternalFailure(
                    "Concurrency failure while updating entities.",
                    e.Message);
            }
            catch (DbUpdateException e)
            {
                return Result<IEnumerable<TDto>>.InternalFailure(
                    "Failed to update entities.",
                    e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(IQuerySpec<TEntity> specification)
        {
            var entity = await specification
                .Apply(Context.Set<TEntity>())
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;

            Context.Set<TEntity>().Remove(entity);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteRangeAsync(IQuerySpec<TEntity> specification)
        {
            var entities = await specification
                .Apply(Context.Set<TEntity>())
                .ToListAsync();

            if (!entities.Any())
                return false;

            Context.Set<TEntity>().RemoveRange(entities);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(IQuerySpec<TEntity> specification)
        {
            return await specification
                .Apply(Context.Set<TEntity>())
                .AnyAsync();
        }
    }
}
