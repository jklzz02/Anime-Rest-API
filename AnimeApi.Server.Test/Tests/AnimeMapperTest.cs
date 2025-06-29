using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.Test.Generators;

namespace AnimeApi.Server.Test.Tests;

public class AnimeMapperTest
{
    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void ToDto_Should_Map_Properties_Correctly(AnimeDto dto, Anime model)
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
        Assert.Equal(dto.Source, result.Source);
        Assert.Equal(dto.TrailerUrl, result.TrailerUrl);
        Assert.Equal(dto.TrailerImageUrl, result.TrailerImageUrl);
        Assert.Equal(dto.TrailerEmbedUrl, result.TrailerEmbedUrl);
        Assert.Equal(dto.Genres, result.Genres);
        Assert.Equal(dto.Licensors, result.Licensors);
        Assert.Equal(dto.Producers, result.Producers);
    }
    
    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoTestData), MemberType = typeof(AnimeGenerator))]
    public void ToDto_Round_Trip_Should_Preserve_Data(AnimeDto dto)
    {
        var model = dto.ToModel();
        var roundTrippedDto = model.ToDto();
    
        Assert.Equal(dto.Id, roundTrippedDto.Id);
        Assert.Equal(dto.Name, roundTrippedDto.Name);
        Assert.Equal(dto.EnglishName, roundTrippedDto.EnglishName);
        Assert.Equal(dto.OtherName, roundTrippedDto.OtherName);
        Assert.Equal(dto.Type, roundTrippedDto.Type);
        Assert.Equal(dto.Source, roundTrippedDto.Source);
        Assert.Equal(dto.Duration, roundTrippedDto.Duration);
        Assert.Equal(dto.ImageUrl, roundTrippedDto.ImageUrl);
        Assert.Equal(dto.Score, roundTrippedDto.Score);
        Assert.Equal(dto.StartedAiring, roundTrippedDto.StartedAiring);
        Assert.Equal(dto.FinishedAiring, roundTrippedDto.FinishedAiring);
        Assert.Equal(dto.ReleaseYear, roundTrippedDto.ReleaseYear);
        Assert.Equal(dto.Synopsis, roundTrippedDto.Synopsis);
        Assert.Equal(dto.Studio, roundTrippedDto.Studio);
        Assert.Equal(dto.Status, roundTrippedDto.Status);
        Assert.Equal(dto.Source, roundTrippedDto.Source);
        Assert.Equal(dto.TrailerUrl, roundTrippedDto.TrailerUrl);
        Assert.Equal(dto.TrailerImageUrl, roundTrippedDto.TrailerImageUrl);
        Assert.Equal(dto.TrailerEmbedUrl, roundTrippedDto.TrailerEmbedUrl);
        Assert.Equal(dto.Genres, roundTrippedDto.Genres);
        Assert.Equal(dto.Licensors, roundTrippedDto.Licensors);
        Assert.Equal(dto.Producers, roundTrippedDto.Producers);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void ToModel_Should_Map_Properties_Correctly(AnimeDto dto, Anime model)
    {
        var result = dto.ToModel();
        
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.English_Name, result.English_Name);
        Assert.Equal(model.Other_Name, result.Other_Name);
        Assert.Equal(model.TypeId, result.TypeId);
        Assert.Equal(model.SourceId, result.SourceId);
        Assert.Equal(model.Duration, result.Duration);
        Assert.Equal(model.Image_URL, result.Image_URL);
        Assert.Equal(model.Score, result.Score);
        Assert.Equal(model.Started_Airing, result.Started_Airing);
        Assert.Equal(model.Finished_Airing, result.Finished_Airing);
        Assert.Equal(model.Release_Year, result.Release_Year);
        Assert.Equal(model.Synopsis, result.Synopsis);
        Assert.Equal(model.Studio, result.Studio);
        Assert.Equal(model.Status, result.Status);
        Assert.Equal(model.Trailer_url, result.Trailer_url);
        Assert.Equal(model.Trailer_image_url, result.Trailer_image_url);
        Assert.Equal(model.Trailer_embed_url, result.Trailer_embed_url);
        Assert.Equal(model.Anime_Genres.Count, result.Anime_Genres.Count);
        Assert.Equal(model.Anime_Licensors.Count, result.Anime_Licensors.Count);
        Assert.Equal(model.Anime_Producers.Count, result.Anime_Producers.Count);
    }
    
    [Fact]
    public void ToDto_Should_Return_Null_When_Model_Is_Null()
    {
        Anime model = null;
        var result = model?.ToDto();
        Assert.Null(result);
    }

    [Fact]
    public void ToModel_Should_Return_Null_When_Dto_Is_Null()
    {
        AnimeDto dto = null;
        var result = dto?.ToModel();
        Assert.Null(result);
    }
    
    [Fact]
    public void ToDto_Should_Map_Empty_Collections_Correctly()
    {
        var model = new Anime
        {
            Anime_Genres = new List<AnimeGenre>(),
            Anime_Licensors = new List<AnimeLicensor>(),
            Anime_Producers = new List<AnimeProducer>()
        };

        var dto = model.ToDto();
    
        Assert.Empty(dto.Genres);
        Assert.Empty(dto.Licensors);
        Assert.Empty(dto.Producers);
    }

}