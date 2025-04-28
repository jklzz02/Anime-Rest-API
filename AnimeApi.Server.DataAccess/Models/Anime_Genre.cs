using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Anime_Genre
{
    public int Id { get; set; }

    public int AnimeId { get; set; }

    public int GenreId { get; set; }

    public virtual Anime Anime { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
