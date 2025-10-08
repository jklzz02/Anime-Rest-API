using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ISourceHelper
{
    Task<SourceDto?> GetByIdAsync(int id);
    Task<IEnumerable<SourceDto>> GetByNameAsync(string name);
    Task<IEnumerable<SourceDto>> GetAllAsync();
    Task<Result<SourceDto>> CreateAsync(SourceDto entity);
    Task<Result<SourceDto>> UpdateAsync(SourceDto entity);
    Task<bool> DeleteAsync(int id);
}