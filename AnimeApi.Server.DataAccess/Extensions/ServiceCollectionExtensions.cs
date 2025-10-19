using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

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
            options.UseNpgsql(new NpgsqlConnection(connectionString), pgSqlOptions => pgSqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IRepository<Anime, AnimeDto>, AnimeRepository>();
        services.AddScoped<IRepository<Producer, ProducerDto>, Repository<Producer, ProducerDto>>();
        services.AddScoped<IRepository<Genre, GenreDto>, Repository<Genre, GenreDto>>();
        services.AddScoped<IRepository<Licensor, LicensorDto>, Repository<Licensor, LicensorDto>>();
        services.AddScoped<IRepository<Type, TypeDto>, Repository<Type, TypeDto>>();
        services.AddScoped<IRepository<Source, SourceDto>, Repository<Source, SourceDto>>();
        services.AddScoped<IRepository<Favourite, FavouriteDto>, Repository<Favourite, FavouriteDto>>();
        services.AddScoped<IRepository<Review, ReviewDto>, Repository<Review, ReviewDto>>();
        services.AddScoped<IRepository<AppUser, AppUserDto>, Repository<AppUser, AppUserDto>>();
        services.AddScoped<IRepository<RefreshToken, RefreshTokenDto>, Repository<RefreshToken, RefreshTokenDto>>();
        services.AddScoped<IRoles, Roles>();
        
        return services;
    }
}