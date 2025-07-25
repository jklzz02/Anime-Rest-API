﻿namespace AnimeApi.Server.Core.Objects.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnimeGenre> Anime_Genres { get; set; } = new List<AnimeGenre>();
}
