using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ISourceHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<SourceDto?> GetByIdAsync(int id);
    Task<IEnumerable<SourceDto>> GetByNameAsync(string name);
    Task<IEnumerable<SourceDto>> GetAllAsync();
    Task<SourceDto?> CreateAsync(SourceDto entity);
    Task<SourceDto?> UpdateAsync(SourceDto entity);
    Task<bool> DeleteAsync(int id);
}