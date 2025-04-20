using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Model;

public partial class Licensor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Anime_Licensor> Anime_Licensors { get; set; } = new List<Anime_Licensor>();
}
