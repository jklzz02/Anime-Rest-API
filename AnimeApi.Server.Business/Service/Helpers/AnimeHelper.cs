using System.Linq.Expressions;
using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions.Mapper;
using AnimeApi.Server.Business.Service.Interfaces;
using AnimeApi.Server.Business.Validator.Interfaces;
using AnimeApi.Server.DataAccess.Model;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using LinqKit;

namespace AnimeApi.Server.Business.Service.Helpers;

public class AnimeHelper : IAnimeHelper
{
    private readonly IAnimeRepository _repository;
    private readonly IAnimeValidator _validator;
    
    public AnimeHelper(IAnimeRepository repository, IAnimeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<AnimeDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByProducerAsync(int producerId)
    {
        var models = await _repository.GetByProducerAsync(producerId);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByLicensorAsync(int licensorId)
    {
        var models = await _repository.GetByLicensorAsync(licensorId);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByGenreAsync(int genreId)
    {
        var models = await _repository.GetByGenreAsync(genreId);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetBySourceAsync(string source)
    {
        var models = await _repository.GetBySourceAsync(source);
        return models.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetByEnglishNameAsync(string englishName)
    {
        var models = await _repository.GetByEnglishNameAsync(englishName);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByScoreAsync(int score)
    {
        var models = await _repository.GetByScoreAsync(score);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByReleaseYearAsync(int year)
    {
        var models = await _repository.GetByReleaseYearAsync(year);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<AnimeDto>> GetByTypeAsync(string type)
    {
        var models = await _repository.GetByTypeAsync(type);
        return models.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var models = await _repository.GetByConditionAsync(condition);
        return models.ToDto();
    }
    
    public async Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var model = await _repository.GetFirstByConditionAsync(condition);
        return model?.ToDto();
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

    public async Task<IEnumerable<AnimeDto>> SearchAsync(AnimeSearchParameters parameters)
    {
        var predicate = PredicateBuilder.New<Anime>(true);

        if (!string.IsNullOrWhiteSpace(parameters.Name))
            predicate = predicate.And(a => a.Name.Contains(parameters.Name));

        if (!string.IsNullOrWhiteSpace(parameters.EnglishName))
            predicate = predicate.And(a => a.English_Name.Contains(parameters.EnglishName));

        if (!string.IsNullOrWhiteSpace(parameters.Source))
            predicate = predicate.And(a => a.Source.Contains(parameters.Source));

        if (!string.IsNullOrWhiteSpace(parameters.Type))
            predicate = predicate.And(a => a.Type.Contains(parameters.Type));

        if (parameters.ProducerId.HasValue)
            predicate = predicate.And(a => a.Anime_Producers.Any(p => p.ProducerId == parameters.ProducerId));

        if (parameters.LicensorId.HasValue)
            predicate = predicate.And(a => a.Anime_Licensors.Any(l => l.LicensorId == parameters.LicensorId));

        if (parameters.GenreId.HasValue)
            predicate = predicate.And(a => a.Anime_Genres.Any(g => g.GenreId == parameters.GenreId));

        if (parameters.MinScore.HasValue)
            predicate = predicate.And(a => a.Score >= parameters.MinScore);

        if (parameters.MaxScore.HasValue)
            predicate = predicate.And(a => a.Score <= parameters.MaxScore);

        if (parameters.MinReleaseYear.HasValue)
            predicate = predicate.And(a => a.Release_Year >= parameters.MinReleaseYear);

        if (parameters.MaxReleaseYear.HasValue)
            predicate = predicate.And(a => a.Release_Year <= parameters.MaxReleaseYear);

        var models = await _repository.GetByConditionAsync(predicate);
        return models.ToDto();
    }
}