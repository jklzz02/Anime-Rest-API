using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IBanService
{
    /// <summary>
    /// Get the active ban linked to the specified user.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <returns>The first <see cref="BanDto"/> linked to the user if any.</returns>
    Task<BanDto?> GetActiveBanAsync(int userId);

    /// <summary>
    /// Get all the active bans linked to the users sharing the specified same email.
    /// </summary>
    /// <param name="email">The email linked with the ban.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all the active bans linked to the email.</returns>
    Task<IEnumerable<BanDto>> GetActiveBansAsync(string email);

    /// <summary>
    /// Permanently ban all user's sharing the specified email.
    /// </summary>
    /// <param name="email">The email of the users to ban.</param>
    /// <param name="reason">The reason for the ban.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of the newly enforced bans.</returns>
    Task<Result<IEnumerable<BanDto>>> PermaBanUser(string email, string reason);

    /// <summary>
    /// Ban all users sharing the specified email.
    /// </summary>
    /// <param name="email">The email of the users to ban.</param>
    /// <param name="expiration">The date of expiration of the ban</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of the newly enforced bans.</returns>
    Task<Result<IEnumerable<BanDto>>> BanUserAsync(string email, DateTime expiration);

    /// <summary>
    /// Ban all users sharing the specified email.
    /// </summary>
    /// <param name="email">The email of the users to ban.</param>
    /// <param name="expiration">The date of expiration of the ban</param>
    /// <param name="reason">The reason for the ban.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of the newly enforced bans.</returns>
    Task<Result<IEnumerable<BanDto>>> BanUserAsync(string email, DateTime? expiration, string? reason);

    /// <summary>
    /// Unban all users sharing the specified email.
    /// </summary>
    /// <param name="email">The email of the users to unban.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of all the newly expired bans.</returns>
    Task<Result<IEnumerable<BanDto>>> UnbanUserAsync(string email);
}