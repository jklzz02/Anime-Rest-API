using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AnimeDbContext _context;
    public GenreRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres
            .OrderBy(g => g.Id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Genre>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Genres
            .Where(g => EF.Functions.Like(g.Name, $"%{name}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetExistingIdsAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .Select(g => g.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetExistingNamesAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .Select(g => g.Name!)
            .ToListAsync();
    }

    public async Task<Result<Genre>> AddAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];

        var genre = await GetByIdAsync(entity.Id);
        if (genre is not null)
        {
            errors.Add(Error.Validation("id", $"Cannot add another anime genre with id '{entity.Id}'"));
        }
        if (_context.Genres.Any(g => g.Name == entity.Name))
        {
            errors.Add(Error.Validation("name", $"Cannot add another anime genre with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Genre>.Failure(errors);
        }
        
        var createdEntry = await _context.Genres.AddAsync(entity);
        var result =  await _context.SaveChangesAsync() > 0;
        if (!result) 
            return Result<Genre>.InternalFailure("create", "something went wrong during entity creation.");
        
        return Result<Genre>.Success(createdEntry.Entity);
    }

    public async Task<Result<Genre>> UpdateAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        List<Error> errors = [];
        
        var genre = await GetByIdAsync(entity.Id);
        if (genre is null)
        {
            errors.Add(Error.Validation("id", $"There is no anime genre with id '{entity.Id}'"));
        }

        if (_context.Genres.Any(g => g.Name == entity.Name && g.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"There is already a anime genre with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Genre>.Failure(errors);
        }
        
        genre.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Genre>.InternalFailure("update", "something went wrong during entity update.");
        
        return Result<Genre>.Success(genre);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var genre = await GetByIdAsync(id);
        if (genre == null) return false;
        _context.Genres.Remove(genre);
        
        return await _context.SaveChangesAsync() > 0;
    }
}