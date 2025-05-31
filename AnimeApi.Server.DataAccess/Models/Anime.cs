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
    
    public string Background { get; set; } = null!;
    
    public string Trailer_image_url { get; set; } = null!;
    
    public string Trailer_url { get; set; } = null!;
    
    public string Trailer_embed_url { get; set; } = null!;

    public virtual ICollection<Anime_Genre> Anime_Genres { get; set; } = new List<Anime_Genre>();

    public virtual ICollection<Anime_Licensor> Anime_Licensors { get; set; } = new List<Anime_Licensor>();

    public virtual ICollection<Anime_Producer> Anime_Producers { get; set; } = new List<Anime_Producer>();

    public virtual Source? Source { get; set; }

    public virtual Type Type { get; set; } = null!;
}
