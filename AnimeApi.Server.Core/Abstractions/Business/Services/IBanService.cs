using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IBanService
{
    Task<BanDto?> GetActiveBanAsync(int userId);

    Task<IEnumerable<BanDto>> GetActiveBansAsync(string email);

    Task<Result<IEnumerable<BanDto>>> PermaBanUser(string email, string reason);

    Task<Result<IEnumerable<BanDto>>> BanUserAsync(string email, DateTime expiration);

    Task<Result<IEnumerable<BanDto>>> BanUserAsync(string email, DateTime? expiration, string? reason);

    Task<Result<IEnumerable<BanDto>>> UnbanUserAsync(string email);
}