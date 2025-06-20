using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Producer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnimeProducer> Anime_Producers { get; set; } = new List<AnimeProducer>();
}
