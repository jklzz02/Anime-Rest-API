using AnimeApi.Server.Business.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface IGenreHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<GenreDto?> GetByIdAsync(int id);
    Task<IEnumerable<GenreDto>> GetByNameAsync(string name);
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<bool> CreateAsync(GenreDto entity);
    Task<bool> UpdateAsync(GenreDto entity);
    Task<bool> DeleteAsync(int id);
}