using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Model;

public partial class Anime_Licensor
{
    public int Id { get; set; }

    public int AnimeId { get; set; }

    public int LicensorId { get; set; }

    public virtual Anime Anime { get; set; } = null!;

    public virtual Licensor Licensor { get; set; } = null!;
}
