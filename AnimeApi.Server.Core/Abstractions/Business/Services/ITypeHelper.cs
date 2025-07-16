using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface ITypeHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<TypeDto?> GetByIdAsync(int id);
    Task<IEnumerable<TypeDto>> GetByNameAsync(string name);
    Task<IEnumerable<TypeDto>> GetAllAsync();
    Task<TypeDto?> CreateAsync(TypeDto entity);
    Task<TypeDto?> UpdateAsync(TypeDto entity);
    Task<bool> DeleteAsync(int id);
}