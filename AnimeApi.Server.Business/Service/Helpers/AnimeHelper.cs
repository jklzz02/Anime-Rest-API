using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions.Mapper;
using AnimeApi.Server.Business.Validator.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Service.Helpers;

public class AnimeHelper
{
    private readonly IAnimeRepository _repository;
    private readonly IAnimeValidator _validator;
    
    public AnimeHelper(IAnimeRepository repository, IAnimeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if(!validationResult.IsValid) return false;
        
        var model = entity.ToModel();
        return await _repository.AddAsync(model);
    }
    
    public async Task<bool> UpdateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if(!validationResult.IsValid) return false;
        
        var model = entity.ToModel();
        return await _repository.UpdateAsync(model);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}