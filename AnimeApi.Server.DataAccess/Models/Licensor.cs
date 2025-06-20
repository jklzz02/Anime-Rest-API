using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Licensor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnimeLicensor> Anime_Licensors { get; set; } = new List<AnimeLicensor>();
}
