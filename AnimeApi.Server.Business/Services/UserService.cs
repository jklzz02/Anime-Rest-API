using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Facades;
using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.Core.Specification;
using FluentValidation;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides user-related services including user retrieval, creation, and deletion.
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
public class UserService(
    IUserFacade userFacade,
    IValidator<FavouriteDto> favouriteValidator) : IUserService
{
    /// <inheritdoc />
    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        return await
            userFacade.Users.FindFirstOrDefaultAsync(
                new UserQuery().ByEmail(email));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AppUserDto>> GetUsersLinkedToEmail(string email)
    {
        return await
            userFacade.Users.FindAsync(
                new UserQuery().ByEmail(email));
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<AppUserDto>> GetUsersAsync(int page, int pageSize)
    {
        var count = await userFacade.Users.CountAsync();
        
        var results = await
            userFacade.Users.FindAsync(new UserQuery()
                .SortByEmail()
                .TieBreaker()
                .Limit(pageSize)
            );
        
        return new PaginatedResult<AppUserDto>(results, page, pageSize, count);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<TUser>> GetUsersAsync<TUser>(int page, int pageSize)
    where TUser : class, IProjectableFrom<AppUserDto>, new()
    {
        var count =  await userFacade.Users.CountAsync();

        var results = await
            userFacade.Users.FindAsync<TUser>(new UserQuery()
                .SortByEmail()
                .TieBreaker()
                .Limit(pageSize)
            );
        
        return new PaginatedResult<TUser>(results, page, pageSize, count);
    }

    /// <inheritdoc />
    public async Task<AppUserDto?> GetByIdAsync(int id)
    {
        var query = new UserQuery().ByPk(id);
        
        return await
            userFacade.Users.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<PublicUser?> GetPublicUserAsync(int id)
    {
        var  query = new UserQuery().ByPk(id);
        return await
            userFacade.Users.FindFirstOrDefaultAsync<PublicUser>(query);
    }

    /// <inheritdoc />
    public async Task<Result<AppUserDto>> GetOrCreateUserAsync(AuthPayload payload)
    {
        var activeBan = await
            userFacade.Bans.FindAsync(
            new BanQuery()
                        .Active()
                        .ByUser(payload.Email));

        if (activeBan.Any())
        {
            return Result<AppUserDto>
                .ValidationFailure(
                    "Banned",
                    $"Email {payload.Email} is linked with a banned account.");
        }
        
        var query = new UserQuery().ByEmail(payload.Email);
        
        var existingUser = await
            userFacade.Users.FindFirstOrDefaultAsync(query);
        
        if (existingUser != null)
        {
            return Result<AppUserDto>.Success(existingUser);
        }

        var newUser = new AppUserDto
        {
            Username = payload.Username,
            Email = payload.Email,
            CreatedAt = DateTime.UtcNow,
            ProfilePictureUrl = payload.Picture,
            Admin = false
        };

        var result = await 
            userFacade.Users.AddAsync(newUser);
        
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(int id)
    {
        var query =  new UserQuery().ByPk(id);
        
        return await
            userFacade.Users.DeleteAsync(query);
    }

    /// <inheritdoc />
    public async Task<bool> DestroyUserAsync(string email)
    {
        var query = new UserQuery().ByEmail(email);
        
        return await 
            userFacade.Users.DeleteAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId)
    {
        var query = new FavouriteQuery().ByUserId(userId);

        return await
            userFacade.Favourites.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<Result<FavouriteDto>> AddFavouriteAsync(FavouriteDto favourite)
    {
        var validationResult = await favouriteValidator.ValidateAsync(favourite);
        if (!validationResult.IsValid)
        {
            return Result<FavouriteDto>.Failure(validationResult.Errors.ToJsonKeyedErrors<FavouriteDto>());
        }

        var result = await userFacade.Favourites.AddAsync(favourite);

        return result.IsSuccess
            ? Result<FavouriteDto>.Success(result.Data)
            : Result<FavouriteDto>.Failure(result.Errors);
    }

    /// <inheritdoc />
    public async Task<bool> RemoveFavouriteAsync(FavouriteDto favourite)
    {
        var query = new FavouriteQuery()
            .ByUserId(favourite.UserId)
            .ByAnimeId(favourite.AnimeId);

        return await 
            userFacade.Favourites.DeleteAsync(query);
    }
}