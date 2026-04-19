using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Specification;

public class IdentityProviderQuery : Specification<IdentityProvider, IdentityProviderQuery>
{
    public IdentityProviderQuery ByPk(int id)
        => FilterBy(i => i.Id == id);
    
    public IdentityProviderQuery ByName(string name)
        => FilterBy(i => i.Name == name);
}