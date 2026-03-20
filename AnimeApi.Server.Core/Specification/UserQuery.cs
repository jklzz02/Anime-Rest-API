using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.Specification;

public class UserQuery : Specification<AppUser, UserQuery>
{
    public UserQuery ByPk(int id)
        => FilterBy(u => u.Id == id);
    
    public UserQuery ByEmail(string email)
        => FilterBy(u => u.Email == email);
    
    public UserQuery ByUsername(string username)
        => FilterBy(u => u.Username == username);

    public UserQuery FullTextSearch(string? query)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            FilterBy(u =>
                EF.Functions.TrigramsAreSimilar(query, u.Username) ||
                EF.Functions.TrigramsAreSimilar(query, u.Email));
        }
        
        return this;
    }
    
    public UserQuery SortByEmail()
        => SortBy(u => u.Email);
    
    public UserQuery TieBreaker()
        => SortBy(u => u.Id);
}