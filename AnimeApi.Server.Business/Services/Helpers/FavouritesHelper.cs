using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

public class FavouritesHelper : IFavouritesHelper
{
    private readonly IRepository<Favourite, FavouriteDto> _repository;
    private readonly IMapper<Favourite, FavouriteDto> _mapper;
    private readonly IValidator<FavouriteDto> _validator;
        
    public FavouritesHelper(
        IRepository<Favourite, FavouriteDto> repository,
        IMapper<Favourite, FavouriteDto> mapper,
        IValidator<FavouriteDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<FavouriteDto?> GetFavouriteAsync(int userId, int animeId)
    {
        var query = new FavouriteQuery()
            .ByUserId(userId)
            .ByAnimeId(animeId)
            .AsNoTracking();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId)
    {
        var query = new FavouriteQuery()
            .ByUserId(userId)
            .AsNoTracking();

        return await
            _repository.FindAsync(query);
    }

    public async Task<Result<FavouriteDto>> AddFavouriteAsync(FavouriteDto favourite)
    {
        var validationResult = await _validator.ValidateAsync(favourite);
        if (!validationResult.IsValid)
        {
            return Result<FavouriteDto>.Failure(validationResult.Errors.ToJsonKeyedErrors<FavouriteDto>());
        }

        var result = await _repository.AddAsync(_mapper.MapToEntity(favourite));

        return result.IsSuccess
            ? Result<FavouriteDto>.Success(result.Data)
            : Result<FavouriteDto>.Failure(result.Errors);
    }

    public async Task<bool> RemoveFavouriteAsync(FavouriteDto favourite)
    {
        var query = new FavouriteQuery()
            .ByUserId(favourite.UserId)
            .ByAnimeId(favourite.AnimeId)
            .AsNoTracking();

        return await 
            _repository.DeleteAsync(query);
    }

    public async Task<int> GetFavouritesCountAsync(int animeId)
    {
        var query = new FavouriteQuery()
            .ByAnimeId(animeId)
            .AsNoTracking();

        return await
            _repository.CountAsync(query);
    }
}