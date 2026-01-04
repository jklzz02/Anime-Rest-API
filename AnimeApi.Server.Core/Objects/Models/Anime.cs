namespace AnimeApi.Server.Core.Objects.Models;

public partial class Anime
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string EnglishName { get; set; } = null!;

    public string OtherName { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int Episodes { get; set; }

    public string Duration { get; set; } = null!;

    public int ReleaseYear { get; set; }

    public DateTime? StartedAiring { get; set; }

    public DateTime? FinishedAiring { get; set; }

    public string Rating { get; set; } = null!;

    public string Studio { get; set; } = null!;

    public decimal Score { get; set; }

    public string Status { get; set; } = null!;

    public int TypeId { get; set; }

    public int? SourceId { get; set; }

    public string? TrailerImageUrl { get; set; }

    public string? TrailerUrl { get; set; }

    public string? TrailerEmbedUrl { get; set; }

    public string? Background { get; set; }

    public virtual ICollection<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();

    public virtual ICollection<AnimeLicensor> AnimeLicensors { get; set; } = new List<AnimeLicensor>();

    public virtual ICollection<AnimeProducer> AnimeProducers { get; set; } = new List<AnimeProducer>();
    
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual Source? Source { get; set; }

    public virtual Type Type { get; set; } = null!;
}
