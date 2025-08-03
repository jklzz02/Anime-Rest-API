using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

public class FavouritesHelper : IFavouritesHelper
{
    private readonly IFavouritesRepository _repository;

    private readonly IValidator<FavouriteDto> _validator;
    
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    
    public FavouritesHelper(
        IFavouritesRepository repository,
        IValidator<FavouriteDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<FavouriteDto?> GetFavouriteAsync(int userId, int animeId)
    {
        var model = await _repository.GetFavouriteAsync(userId, animeId);
        return model?.ToDto();
    }

    public async Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId)
    {
        var models = await _repository.GetFavouritesAsync(userId);
        return models.Select(animeId => new FavouriteDto
        {
            UserId = userId,
            AnimeId = animeId
        });
    }

    public async Task<bool> AddFavouriteAsync(FavouriteDto favourite)
    {
        var validationResult = await _validator.ValidateAsync(favourite);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<FavouriteDto>();
            return false;
        }
        
        return await _repository.AddFavouriteAsync(favourite.UserId, favourite.AnimeId);
    }

    public async Task<bool> RemoveFavouriteAsync(FavouriteDto favourite)
    {
        var validationResult = await _validator.ValidateAsync(favourite);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<FavouriteDto>();
            return false;
        }
        
        return await _repository.RemoveFavouriteAsync(favourite.UserId, favourite.AnimeId);
    }

    public async Task<int> GetFavouritesCountAsync(int animeId)
    {
        return await _repository.GetFavouritesCountAsync(animeId);
    }
}