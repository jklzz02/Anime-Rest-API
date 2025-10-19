using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.SpecHelpers;

public class UserQuery : QuerySpec<AppUser, UserQuery>
{
    public UserQuery ByPk(int id)
        => FilterBy(u => u.Id == id);
    
    public UserQuery ByEmail(string email)
        => FilterBy(u => u.Email == email);
}