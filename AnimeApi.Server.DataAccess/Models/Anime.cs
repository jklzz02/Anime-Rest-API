using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Anime
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string English_Name { get; set; } = null!;

    public string Other_Name { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public string Image_URL { get; set; } = null!;

    public int Episodes { get; set; }

    public string Duration { get; set; } = null!;

    public int Release_Year { get; set; }

    public DateOnly? Started_Airing { get; set; }

    public DateOnly? Finished_Airing { get; set; }

    public string Rating { get; set; } = null!;

    public string Studio { get; set; } = null!;

    public decimal Score { get; set; }

    public string Status { get; set; } = null!;

    public int TypeId { get; set; }

    public int? SourceId { get; set; }

    public string? Trailer_image_url { get; set; }

    public string? Trailer_url { get; set; }

    public string? Trailer_embed_url { get; set; }

    public string? Background { get; set; }

    public virtual ICollection<AnimeGenre> Anime_Genres { get; set; } = new List<AnimeGenre>();

    public virtual ICollection<AnimeLicensor> Anime_Licensors { get; set; } = new List<AnimeLicensor>();

    public virtual ICollection<AnimeProducer> Anime_Producers { get; set; } = new List<AnimeProducer>();
    
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Source? Source { get; set; }

    public virtual Type Type { get; set; } = null!;
}
