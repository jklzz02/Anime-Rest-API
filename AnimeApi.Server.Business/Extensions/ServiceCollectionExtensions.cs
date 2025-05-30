using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators;
using AnimeApi.Server.Business.Validators.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddTransient<IAnimeValidator, AnimeValidator>();
        services.AddTransient<IBaseValidator<GenreDto>, BaseValidator<GenreDto>>();
        services.AddTransient<IBaseValidator<SourceDto>, BaseValidator<SourceDto>>();
        services.AddTransient<IBaseValidator<TypeDto>, BaseValidator<TypeDto>>();
        services.AddTransient<IBaseValidator<ProducerDto>, BaseValidator<ProducerDto>>();
        services.AddTransient<IBaseValidator<LicensorDto>, BaseValidator<LicensorDto>>();
        
        services.AddTransient<IAnimeHelper, AnimeHelper>();
        services.AddTransient<IGenreHelper, GenreHelper>();
        services.AddTransient<IProducerHelper, ProducerHelper>();
        services.AddTransient<ILicensorHelper, LicensorHelper>();
        services.AddTransient<ISourceHelper, SourceHelper>();
        services.AddTransient<ITypeHelper, TypeHelper>();

        services.AddTransient<ICachingService, CachingService>();
        
        return services;
    }
}