using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ILicensorHelper
{
    Task<LicensorDto?> GetByIdAsync(int id);
    Task<IEnumerable<LicensorDto>> GetByNameAsync(string name);
    Task<IEnumerable<LicensorDto>> GetAllAsync();
    Task<Result<LicensorDto>> CreateAsync(LicensorDto entity);
    Task<Result<LicensorDto>> UpdateAsync(LicensorDto entity);
    Task<bool> DeleteAsync(int id);
}