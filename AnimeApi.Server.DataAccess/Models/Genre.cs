using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Anime_Genre> Anime_Genres { get; set; } = new List<Anime_Genre>();
}
