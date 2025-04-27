using AnimeApi.Server.Business.Dto;

namespace AnimeApi.Server.Business.Service.Interfaces;

public interface IProducerHelper
{
    Task<ProducerDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProducerDto>> GetByNameAsync(string name);
    Task<IEnumerable<ProducerDto>> GetAllAsync();
    Task<bool> CreateAsync(ProducerDto entity);
    Task<bool> UpdateAsync(ProducerDto entity);
    Task<bool> DeleteAsync(int id);
}