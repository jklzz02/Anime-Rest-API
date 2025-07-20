using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Repositories;
using AnimeApi.Server.DataAccess.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeApi.Server.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds data access services and configures the database context for the given service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services are added.</param>
    /// <param name="connectionString">The connection string to configure the database context.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with data access services registered.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AnimeDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions => mySqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IAnimeRepository, AnimeRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ILicensorRepository, LicensorRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        
        return services;
    }
}