using System;
using System.Collections.Generic;

namespace AnimeApi.Server.DataAccess.Models;

public partial class AnimeFullInfo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string English_Name { get; set; } = null!;

    public string Other_Name { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public int Episodes { get; set; }

    public string Type { get; set; } = null!;

    public string? Source { get; set; }

    public string Duration { get; set; } = null!;

    public int Release_Year { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly? Started_Airing { get; set; }

    public DateOnly? Finished_Airing { get; set; }

    public string Rating { get; set; } = null!;

    public decimal Score { get; set; }

    public string Studio { get; set; } = null!;

    public string? Genres { get; set; }

    public string? Licensors { get; set; }

    public string? Producers { get; set; }

    public string Image_URL { get; set; } = null!;
}
