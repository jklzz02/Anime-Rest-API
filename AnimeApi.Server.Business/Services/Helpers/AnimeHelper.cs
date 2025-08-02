using System.Linq.Expressions;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class AnimeHelper : IAnimeHelper
{
    private readonly IAnimeRepository _repository;
    private readonly IAnimeValidator _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
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

    public async Task<PaginatedResult<AnimeDto>?> GetAllAsync(int page, int size)
    {
        var result = await _repository
            .GetAllAsync(page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<IEnumerable<AnimeDto>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var models = await _repository.GetByIdsAsync(ids);
        return models.ToDto();
    }

    public async Task<PaginatedResult<AnimeDto>?> GetAllNonAdultAsync(int page, int size)
    {
        var result = await _repository
            .GetAllNonAdultAsync(page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByNameAsync(string name, int page, int size = 100)
    {
        var result = await _repository.GetByNameAsync(name, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByProducerAsync(int producerId, int page, int size = 100)
    {
        var result = await _repository.GetByProducerAsync(producerId, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByLicensorAsync(int licensorId, int page, int size = 100)
    {
        var result = await _repository.GetByLicensorAsync(licensorId, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByGenreAsync(int genreId, int page, int size = 100)
    {
        var result = await _repository.GetByGenreAsync(genreId, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetBySourceAsync(string source, int page, int size = 100)
    {
        var result = await _repository.GetBySourceAsync(source, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<PaginatedResult<AnimeDto>?> GetByEnglishNameAsync(string englishName, int page, int size = 100)
    {
        var result = await _repository.GetByEnglishNameAsync(englishName, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByScoreAsync(int score, int page, int size = 100)
    {
        var result = await _repository.GetByScoreAsync(score, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByReleaseYearAsync(int year, int page, int size = 100)
    {
        var result = await _repository.GetByReleaseYearAsync(year, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>?> GetByTypeAsync(string type, int page, int size = 100)
    {
        var result = await _repository.GetByTypeAsync(type, page, size);
        
        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<PaginatedResult<AnimeDto>?> GetByEpisodesAsync(int episodes, int page, int size = 100)
    {
        var result = await _repository.GetByEpisodesAsync(episodes, page, size);

        if (_repository.ErrorMessages.Any())
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    
    public async Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var model = await _repository.GetFirstByConditionAsync(condition);
        return model?.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count)
    {
        if (count <= 0)
        {
            ErrorMessages.Add("Count", "Count must be greater than 0.");
        }

        if (ErrorMessages.Any())
        {
            return [];
        }
        
        var models = await _repository.GetMostRecentAsync(count);
        
        return models.ToDto();
    }

    public async Task<IEnumerable<AnimeSummaryDto>> GetSummariesAsync(int count)
    {
        if (count <= 0)
        {
            ErrorMessages.Add("Count", "Count must be greater than 0.");
            return [];       
        }
        
        var models = await _repository
            .GetSummariesAsync(count);

        return models.ToDto();
    }

    public async Task<AnimeDto?> CreateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<AnimeDto>();
            return null;
        }
        
        var model = entity.ToModel(false);
        var result = await _repository.AddAsync(model);
        if(result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        return result.ToDto();
    }
    
    public async Task<AnimeDto?> UpdateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<AnimeDto>();
            return null;
        }
        
        var model = entity.ToModel();
        var result = await _repository.UpdateAsync(model);
        if(result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.ToDto();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<PaginatedResult<AnimeDto>?> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100)
    {
        var filters = new List<Expression<Func<Anime, bool>>>();

        if (!string.IsNullOrWhiteSpace(parameters.Name))
            filters.Add(a => a.Name.Contains(parameters.Name));

        if (!string.IsNullOrWhiteSpace(parameters.EnglishName))
            filters.Add(a => a.English_Name.Contains(parameters.EnglishName));

        if (!string.IsNullOrWhiteSpace(parameters.Source))
            filters.Add(a=> a.Source.Name.Contains(parameters.Source));

        if (!string.IsNullOrWhiteSpace(parameters.Type))
            filters.Add(a => a.Type.Name.Contains(parameters.Type));

        if (parameters.ProducerId.HasValue)
            filters.Add(a => a.Anime_Producers.Any(p => p.ProducerId == parameters.ProducerId));
        
        if (!string.IsNullOrWhiteSpace(parameters.ProducerName))
            filters.Add(a => a.Anime_Producers.Any(p => p.Producer.Name.Contains(parameters.ProducerName)));

        if (parameters.ProducerNames?.Any() ?? false)
        {
            filters.Add(a => parameters
                .ProducerNames.All(p => a.Anime_Producers.Any(ap => ap.Producer.Name == p)));
        }
        
        if (parameters.LicensorId.HasValue)
            filters.Add(a => a.Anime_Licensors.Any(l => l.LicensorId == parameters.LicensorId));

        if (!string.IsNullOrWhiteSpace(parameters.LicensorName))
            filters.Add(a => a.Anime_Licensors.Any(l => l.Licensor.Name.Contains(parameters.LicensorName)));

        if (parameters.LicensorNames?.Any() ?? false)
        {
            filters.Add(a => parameters
                .LicensorNames.All(l => a.Anime_Licensors.Any(al => al.Licensor.Name == l)));
        }
        
        if (parameters.GenreId.HasValue)
            filters.Add(a => a.Anime_Genres.Any(g => g.GenreId == parameters.GenreId));
        
        if (!string.IsNullOrWhiteSpace(parameters.GenreName))
            filters.Add(a => a.Anime_Genres.Any(g => g.Genre.Name.Contains(parameters.GenreName)));

        if (parameters.GenreNames?.Any() ?? false)
        {
            filters.Add(a => parameters
                .GenreNames.All(g => a.Anime_Genres.Any(ag => ag.Genre.Name == g)));
        }

        if (parameters.Episodes.HasValue)
            filters.Add(a => a.Episodes == parameters.Episodes);
        
        if (parameters.MinScore.HasValue)
            filters.Add(a => a.Score >= parameters.MinScore);

        if (parameters.MaxScore.HasValue)
            filters.Add(a => a.Score <= parameters.MaxScore);

        if (parameters.MinReleaseYear.HasValue)
            filters.Add(a => a.Release_Year >= parameters.MinReleaseYear);

        if (parameters.MaxReleaseYear.HasValue)
            filters.Add(a => a.Release_Year <= parameters.MaxReleaseYear && a.Release_Year != 0);
        
        if (!parameters.IncludeAdultContext)
            filters.Add(a => !string.IsNullOrEmpty(a.Rating) && !a.Rating.ToLower().Contains(Constants.Ratings.AdultContent));

        var result = await _repository.GetByConditionAsync(page, size, filters);

        if (_repository.ErrorMessages?.Any() ?? false)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
}