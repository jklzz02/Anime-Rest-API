using AnimeApi.Server.Business.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface ILicensorHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<LicensorDto?> GetByIdAsync(int id);
    Task<IEnumerable<LicensorDto>> GetByNameAsync(string name);
    Task<IEnumerable<LicensorDto>> GetAllAsync();
    Task<bool> CreateAsync(LicensorDto entity);
    Task<bool> UpdateAsync(LicensorDto entity);
    Task<bool> DeleteAsync(int id);
}