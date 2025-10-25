
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.Extensions.DependencyInjection;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Core.Extensions;
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds mapper dependencies to the IServiceCollection.
    /// Registers various mapper interfaces and their corresponding implementations for dependency injection.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the mapper services are to be added.</param>
    /// <returns>The IServiceCollection with the mapper services added.</returns>
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IAnimeMapper, AnimeMapper>();
        services.AddScoped<IMapper<Producer, ProducerDto>, BaseMapper<Producer, ProducerDto>>();
        services.AddScoped<IMapper<Licensor, LicensorDto>, BaseMapper<Licensor, LicensorDto>>();
        services.AddScoped<IMapper<Genre, GenreDto>, BaseMapper<Genre, GenreDto>>();
        services.AddScoped<IMapper<Type, TypeDto>, BaseMapper<Type, TypeDto>>();
        services.AddScoped<IMapper<Source, SourceDto>, BaseMapper<Source, SourceDto>>();
        services.AddScoped<IMapper<Favourite, FavouriteDto>, FavouritesMapper>();
        services.AddScoped<IMapper<Review, ReviewDto>, ReviewMapper>();
        services.AddScoped<IMapper<RefreshToken, RefreshTokenDto>, RefreshTokenMapper>();
        services.AddScoped<IMapper<AppUser, AppUserDto>, AppUserMapper>();

        return services;
    }
}
