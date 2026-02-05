using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Business.Validators;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Objects;
using Microsoft.Extensions.DependencyInjection;
using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds business-level services, helpers, and validators to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> configured with business-level dependencies.</returns>
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddTransient<IValidator<AnimeDto>, AnimeValidator>();
        services.AddTransient<IValidator<ReviewDto>, ReviewValidator>();
        services.AddTransient<IValidator<AnimeSearchParameters>, AnimeSearchParametersValidator>();
        services.AddTransient<IValidator<FavouriteDto>, FavouriteValidator>();
        services.AddTransient<IBaseValidator<GenreDto>, BaseValidator<GenreDto>>();
        services.AddTransient<IBaseValidator<SourceDto>, BaseValidator<SourceDto>>();
        services.AddTransient<IBaseValidator<TypeDto>, BaseValidator<TypeDto>>();
        services.AddTransient<IBaseValidator<ProducerDto>, BaseValidator<ProducerDto>>();
        services.AddTransient<IBaseValidator<LicensorDto>, BaseValidator<LicensorDto>>();
        
        services.AddScoped<IAnimeHelper, AnimeHelper>();
        services.AddScoped<IGenreHelper, GenreHelper>();
        services.AddScoped<IProducerHelper, ProducerHelper>();
        services.AddScoped<ILicensorHelper, LicensorHelper>();
        services.AddScoped<ISourceHelper, SourceHelper>();
        services.AddScoped<ITypeHelper, TypeHelper>();
        services.AddScoped<IFavouritesHelper, FavouritesHelper>();
        services.AddScoped<IReviewHelper, ReviewHelper>();
        
        services.AddScoped<IBanService, BanService>();
        services.AddSingleton<ICachingService, CachingService>();
        
        return services;
    }

    /// <summary>
    /// Adds identity-related services, including authentication and user management,
    /// to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the identity services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> configured with identity-related dependencies.</returns>
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRefreshTokenService,RefreshTokenService>();
        services.AddScoped<ISocialAuthService, SocialAuthService>();
        
        services.AddSingleton<ITokenHasher, TokenHasher>();
        
        services.AddAuthenticationCore();

        return services;
    }
}