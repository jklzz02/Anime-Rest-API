using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Specification;

namespace AnimeApi.Server.Business.Services;

public class BanService(
    IRepository<AppUser, AppUserDto> userRepo,
    IRepository<Ban, BanDto> banRepo)
    : IBanService
{
    public async Task<BanDto?> GetActiveBanAsync(int userId)
    {
        var query = new BanQuery()
            .ByUser(userId)
            .Active();

        return await
            banRepo.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<BanDto>> GetActiveBansAsync(string email)
    {
        var query = new BanQuery()
            .ByUser(email.ToLowerNormalized())
            .Active();
        
        return await
            banRepo.FindAsync(query);
    }
    
    public async Task<Result<IEnumerable<BanDto>>> PermaBanUser(string email, string reason)
        => await BanUserAsync(email, null, reason);
    
    public async Task<Result<IEnumerable<BanDto>>> BanUserAsync(string email, DateTime expiration)
        => await BanUserAsync(email, expiration, null);
    
    public async Task<Result<IEnumerable<BanDto>>> BanUserAsync(
        string email,
        DateTime? expiration,
        string? reason)
    {
        if (expiration < DateTime.UtcNow)
        {
            return Result<IEnumerable<BanDto>>
                .ValidationFailure(
                "Expired",
                "Cannot set expiration in the past.");
        }
        
        var user = (await
            userRepo.FindAsync(new UserQuery().ByEmail(email)))
                .ToList();

        if (!user.Any())
        {
            return Result<IEnumerable<BanDto>>.ValidationFailure(
                "User not found",
                $"There is no user with email '{email}'");
        }
        
        var activeBan = (await GetActiveBansAsync(email)).ToList();

        if (!activeBan.Any())
        {
            
            var newEntries = user.Select(u =>
                new BanDto 
                {
                    UserId = u.Id,
                    NormalizedEmail =  u.Email.ToLowerNormalized(),
                    CreatedAt =  DateTime.UtcNow,
                    Expiration =  expiration,
                    Reason = reason
                });
        
            return await
                banRepo.AddRangeAsync(newEntries);   
        }
        
        var updatedEntries = activeBan
            .Select(ab => ab.Updated(reason, expiration));
        
        return await 
            banRepo.UpdateRangeAsync(updatedEntries);
    }
    
    public async Task<Result<IEnumerable<BanDto>>> UnbanUserAsync(string email)
    {
        var activeBan =  (await
            banRepo.FindAsync(new BanQuery()
                .ByUser(email.ToLowerNormalized())
                .Active()))
            .ToList();

        if (!activeBan.Any())
        {
            return Result<IEnumerable<BanDto>>.ValidationFailure(
                "User not found",
                $"There is no active ban for user with email '{email}'.");
        }

        var updatedEntries = activeBan.Select(b => b.Revoked());
        
        return await
            banRepo.UpdateRangeAsync(updatedEntries);
    }
}