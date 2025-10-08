using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IProducerHelper
{
    Task<ProducerDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProducerDto>> GetByNameAsync(string name);
    Task<IEnumerable<ProducerDto>> GetAllAsync();
    Task<Result<ProducerDto>> CreateAsync(ProducerDto entity);
    Task<Result<ProducerDto>> UpdateAsync(ProducerDto entity);
    Task<bool> DeleteAsync(int id);
}