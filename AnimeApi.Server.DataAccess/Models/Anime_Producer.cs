using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Anime_Producer
{
    public int Id { get; set; }

    public int AnimeId { get; set; }

    public int ProducerId { get; set; }

    public virtual Anime Anime { get; set; } = null!;

    public virtual Producer Producer { get; set; } = null!;
}
