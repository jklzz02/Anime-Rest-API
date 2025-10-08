using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ITypeHelper
{
    Task<TypeDto?> GetByIdAsync(int id);
    Task<IEnumerable<TypeDto>> GetByNameAsync(string name);
    Task<IEnumerable<TypeDto>> GetAllAsync();
    Task<Result<TypeDto>> CreateAsync(TypeDto entity);
    Task<Result<TypeDto>> UpdateAsync(TypeDto entity);
    Task<bool> DeleteAsync(int id);
}