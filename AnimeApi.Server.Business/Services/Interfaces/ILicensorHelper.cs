using AnimeApi.Server.Business.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface ILicensorHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<LicensorDto?> GetByIdAsync(int id);
    Task<IEnumerable<LicensorDto>> GetByNameAsync(string name);
    Task<IEnumerable<LicensorDto>> GetAllAsync();
    Task<LicensorDto?> CreateAsync(LicensorDto entity);
    Task<LicensorDto?> UpdateAsync(LicensorDto entity);
    Task<bool> DeleteAsync(int id);
}