using AnimeApi.Server.Core.Objects.Partials;

namespace AnimeApi.Server.Core.Objects.Dto;

public record AppUserDetails
{
    public int Id { get; init; }
    
    public string Username { get; init; } = string.Empty;
    
    public string PictureUrl { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public bool Admin { get; init; }
    
    public IEnumerable<AppUserDto> LinkedUsers { get; init; } = [];
    
    public IEnumerable<AnimeListItem> Favourites { get; init; } = [];
    
    public IEnumerable<ReviewDto> Reviews { get; init; } = [];
}