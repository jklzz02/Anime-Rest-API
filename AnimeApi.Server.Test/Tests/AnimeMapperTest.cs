using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.Test.Generators;

namespace AnimeApi.Server.Test.Tests;

public class AnimeMapperTest
{
    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void To_Dto_Should_Map_Properties_Correctly(AnimeDto dto, Anime model)
    {
        var result = model.ToDto();
        
        Assert.Equal(dto.Id, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.EnglishName, result.EnglishName);
        Assert.Equal(dto.OtherName, result.OtherName);
        Assert.Equal(dto.Type, result.Type);
        Assert.Equal(dto.Duration, result.Duration);
        Assert.Equal(dto.ImageUrl, result.ImageUrl);
        Assert.Equal(dto.Score, result.Score);
        Assert.Equal(dto.StartedAiring, result.StartedAiring);
        Assert.Equal(dto.FinishedAiring, result.FinishedAiring);
        Assert.Equal(dto.ReleaseYear, result.ReleaseYear);
        Assert.Equal(dto.Synopsis, result.Synopsis);
        Assert.Equal(dto.Studio, result.Studio);
        Assert.Equal(dto.Status, result.Status);
        Assert.Equal(dto.Genres, result.Genres);
        Assert.Equal(dto.Licensors, result.Licensors);
        Assert.Equal(dto.Producers, result.Producers);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void To_Model_Should_Map_Properties_Correctly(AnimeDto dto, Anime model)
    {
        var result = dto.ToModel();
        
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.English_Name, result.English_Name);
        Assert.Equal(model.Other_Name, result.Other_Name);
        Assert.Equal(model.Type, result.Type);
        Assert.Equal(model.Duration, result.Duration);
        Assert.Equal(model.Image_URL, result.Image_URL);
        Assert.Equal(model.Score, result.Score);
        Assert.Equal(model.Started_Airing, result.Started_Airing);
        Assert.Equal(model.Finished_Airing, result.Finished_Airing);
        Assert.Equal(model.Release_Year, result.Release_Year);
        Assert.Equal(model.Synopsis, result.Synopsis);
        Assert.Equal(model.Studio, result.Studio);
        Assert.Equal(model.Status, result.Status);

        Assert.Equal(model.Anime_Genres.Count, result.Anime_Genres.Count);
        Assert.Equal(model.Anime_Licensors.Count, result.Anime_Licensors.Count);
        Assert.Equal(model.Anime_Producers.Count, result.Anime_Producers.Count);
    }
    
}