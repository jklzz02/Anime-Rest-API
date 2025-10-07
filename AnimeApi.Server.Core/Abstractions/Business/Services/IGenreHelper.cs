using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IGenreHelper
{
    Task<GenreDto?> GetByIdAsync(int id);
    Task<IEnumerable<GenreDto>> GetByNameAsync(string name);
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<Result<GenreDto>> CreateAsync(GenreDto entity);
    Task<Result<GenreDto>> UpdateAsync(GenreDto entity);
    Task<bool> DeleteAsync(int id);
}