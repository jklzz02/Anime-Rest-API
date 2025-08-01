namespace AnimeApi.Server.Core.Abstractions.DataAccess.Models;

public interface IBaseModel
{
    int Id { get; set; }
    string Name { get; set; }
}