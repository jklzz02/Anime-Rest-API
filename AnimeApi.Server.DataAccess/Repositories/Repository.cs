using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories
{
    public class Repository<TEntity, TDto> : IRepository<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected AnimeDbContext Context {  get; }
        protected readonly IMapper<TEntity, TDto> _mapper;

        public Repository(AnimeDbContext context, IMapper<TEntity, TDto> mapper)
        {
            Context = context;
            _mapper = mapper;
        }

        public async Task<int> CountAsync(IQuerySpec<TEntity> specification)
        {
            return await
                specification
                    .Apply(Context.Set<TEntity>())
                    .CountAsync();
        }


        public async Task<IEnumerable<TDto>> FindAsync(IQuerySpec<TEntity> specification)
        {
            List<TEntity> result = await
                specification
                    .Apply(Context.Set<TEntity>())
                    .ToListAsync();

            return _mapper.MapToDto(result);
        }

        public async Task<TDto?> FindFirstOrDefaultAsync(IQuerySpec<TEntity> specification)
        {
            TEntity? result = await
                specification
                    .Apply(Context.Set<TEntity>())
                    .FirstOrDefaultAsync();

            return result is null
                ? null
                : _mapper.MapToDto(result);
        }
        

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            List<TEntity> result = await
                Context.Set<TEntity>()
                    .AsNoTracking()
                    .ToListAsync();

            return _mapper.MapToDto(result);
        }

        public virtual async Task<Result<TDto>> AddAsync(TEntity entity)
        {
            Detach(entity);
            
           var createdEntity = await 
                Context.Set<TEntity>()
                    .AddAsync(entity);

            bool saveResult = await Context.SaveChangesAsync() > 0;

            return saveResult
                ? Result<TDto>.Success(_mapper.MapToDto(createdEntity.Entity))
                : Result<TDto>.InternalFailure("Failed to add entity.", "An error occurred while saving the entity to the database.");
        }

        public async Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            Detach(entity);
            
            await Context.Set<TEntity>().AddRangeAsync(entity);
            bool saveResult = await Context.SaveChangesAsync() > 0;

            return saveResult
                ? Result<IEnumerable<TDto>>.Success(_mapper.MapToDto(entity))
                : Result<IEnumerable<TDto>>.InternalFailure("Failed to add entities.", "An error occurred while saving the entities to the database.");
        }

        public virtual async Task<Result<TDto>> UpdateAsync(TEntity entity)
        {
            Detach(entity);
            
            Context.Set<TEntity>().Update(entity);
            bool saveResult = await Context.SaveChangesAsync() > 0;
            return saveResult
                ? Result<TDto>.Success(_mapper.MapToDto(entity))
                : Result<TDto>.InternalFailure("Failed to update entity.", "An error occurred while saving the entity to the database.");
        }

        public async Task<Result<IEnumerable<TDto>>> UpdateRangeAsync(IEnumerable<TEntity> entity)
        {
            Detach(entity);
            
            Context.Set<TEntity>().UpdateRange(entity);
            bool saveResult = await Context.SaveChangesAsync() > 0;
            return saveResult
                ? Result<IEnumerable<TDto>>.Success(_mapper.MapToDto(entity))
                : Result<IEnumerable<TDto>>.InternalFailure("Failed to update entities.", "An error occurred while saving the entities to the database.");
        }

        public virtual async Task<bool> DeleteAsync(IQuerySpec<TEntity> specification)
        {
            TDto? result = await
                FindFirstOrDefaultAsync(specification);

            if (result is null)
            {
                return false;
            }

            Context.Set<TEntity>().Remove(_mapper.MapToEntity(result)!);

            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(IQuerySpec<TEntity> specification)
        {
            var result = await
                FindAsync(specification);

            if (!result.Any())
            {
                return false;
            }

            Context.Set<TEntity>().RemoveRange(_mapper.MapToEntity(result)!);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        public async Task<bool> ExistsAsync(IQuerySpec<TEntity> specification)
        {
            return await CountAsync(specification) > 0;
        }

        protected void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        protected void Detach(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(Detach);
        }
    }
}
