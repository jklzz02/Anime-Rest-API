using AnimeApi.Server.Core.Abstractions.Dto;

namespace AnimeApi.Server.Core.Objects.Dto;

public class IdentityProviderDto : IBaseDto
{
    public int? Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
}