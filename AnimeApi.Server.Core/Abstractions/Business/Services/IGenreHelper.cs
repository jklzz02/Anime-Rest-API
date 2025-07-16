using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IGenreHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<GenreDto?> GetByIdAsync(int id);
    Task<IEnumerable<GenreDto>> GetByNameAsync(string name);
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<GenreDto?> CreateAsync(GenreDto entity);
    Task<GenreDto?> UpdateAsync(GenreDto entity);
    Task<bool> DeleteAsync(int id);
}