namespace AnimeApi.Server.Core.Abstractions.DataAccess.Models;

public interface IBaseEntity
{
    int Id { get; set; }
    string Name { get; set; }
}