using AnimeApi.Server.Business.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface IProducerHelper
{
    public Dictionary<string, string> ErrorMessages { get; }
    Task<ProducerDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProducerDto>> GetByNameAsync(string name);
    Task<IEnumerable<ProducerDto>> GetAllAsync();
    Task<ProducerDto?> CreateAsync(ProducerDto entity);
    Task<ProducerDto?> UpdateAsync(ProducerDto entity);
    Task<bool> DeleteAsync(int id);
}