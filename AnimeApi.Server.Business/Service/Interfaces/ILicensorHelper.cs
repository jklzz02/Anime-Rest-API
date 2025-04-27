using AnimeApi.Server.Business.Dto;

namespace AnimeApi.Server.Business.Service.Interfaces;

public interface ILicensorHelper
{
    Task<LicensorDto?> GetByIdAsync(int id);
    Task<IEnumerable<LicensorDto>> GetByNameAsync(string name);
    Task<IEnumerable<LicensorDto>> GetAllAsync();
    Task<bool> CreateAsync(LicensorDto entity);
    Task<bool> UpdateAsync(LicensorDto entity);
    Task<bool> DeleteAsync(int id);
}