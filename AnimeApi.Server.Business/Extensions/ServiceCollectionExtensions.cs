using AnimeApi.Server.Business.Services;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators;
using AnimeApi.Server.Business.Validators.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeApi.Server.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddTransient<IAnimeValidator, AnimeValidator>();
        services.AddTransient<IGenreValidator, GenreValidator>();
        services.AddTransient<IProducerValidator, ProducerValidator>();
        services.AddTransient<ILicensorValidator, LicensorValidator>();
        
        services.AddTransient<IAnimeHelper, AnimeHelper>();
        services.AddTransient<IGenreHelper, GenreHelper>();
        services.AddTransient<IProducerHelper, ProducerHelper>();
        services.AddTransient<ILicensorHelper, LicensorHelper>();

        services.AddTransient<ICachingService, CachingService>();
        
        return services;
    }
}