using AnimeApi.Server.Core.Mappers;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.Test.Generators;

namespace AnimeApi.Server.Test.Tests;

public class AnimeMapperTest
{
    private AnimeMapper _mapper
        => new AnimeMapper(
            new BaseMapper<Producer, ProducerDto>(),
            new BaseMapper<Licensor, LicensorDto>(),
            new BaseMapper<Genre, GenreDto>());
    
    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void ToDto_Should_Map_Properties_Correctly(AnimeDto dto, Anime model)
    {
        var result = _mapper.MapToDto(model);
        
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
        var model = _mapper.MapToEntity(dto);
        var roundTrippedDto = _mapper.MapToDto(model);
    
        Assert.Equal(dto.Id, roundTrippedDto.Id);
        Assert.Equal(dto.Name, roundTrippedDto.Name);
        Assert.Equal(dto.EnglishName, roundTrippedDto.EnglishName);
        Assert.Equal(dto.OtherName, roundTrippedDto.OtherName);
        Assert.Equal(dto.Type, roundTrippedDto.Type);
        Assert.Equal(dto.Source, roundTrippedDto.Source);
        Assert.Equal(dto.Duration, roundTrippedDto.Duration);
        Assert.Equal(dto.ImageUrl, roundTrippedDto.ImageUrl);
        Assert.Equal(dto.Score, roundTrippedDto.Score);
        Assert.Equal(dto.StartedAiring?.ToUniversalTime(), roundTrippedDto.StartedAiring);
        Assert.Equal(dto.FinishedAiring?.ToUniversalTime(), roundTrippedDto.FinishedAiring);
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
        var result = _mapper.MapToEntity(dto);
        
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.EnglishName, result.EnglishName);
        Assert.Equal(model.OtherName, result.OtherName);
        Assert.Equal(model.TypeId, result.TypeId);
        Assert.Equal(model.SourceId, result.SourceId);
        Assert.Equal(model.Duration, result.Duration);
        Assert.Equal(model.ImageUrl, result.ImageUrl);
        Assert.Equal(model.Score, result.Score);
        Assert.Equal(model.StartedAiring?.ToUniversalTime(), result.StartedAiring);
        Assert.Equal(model.FinishedAiring?.ToUniversalTime(), result.FinishedAiring);
        Assert.Equal(model.ReleaseYear, result.ReleaseYear);
        Assert.Equal(model.Synopsis, result.Synopsis);
        Assert.Equal(model.Studio, result.Studio);
        Assert.Equal(model.Status, result.Status);
        Assert.Equal(model.TrailerUrl, result.TrailerUrl);
        Assert.Equal(model.TrailerImageUrl, result.TrailerImageUrl);
        Assert.Equal(model.TrailerEmbedUrl, result.TrailerEmbedUrl);
        Assert.Equal(model.AnimeGenres.Count, result.AnimeGenres.Count);
        Assert.Equal(model.AnimeLicensors.Count, result.AnimeLicensors.Count);
        Assert.Equal(model.AnimeProducers.Count, result.AnimeProducers.Count);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public void Projection_To_Summary_Should_Work_Correctly(Anime model)
    {
        AnimeSummary projectedSummary = _mapper.ProjectTo<AnimeSummary>(model);
        Assert.Equal(model.Id, projectedSummary.Id);
        Assert.Equal(model.Name, projectedSummary.Name);
        Assert.Equal(model.ImageUrl, projectedSummary.ImageUrl);
        Assert.Equal(model.Score, projectedSummary.Score);
        Assert.Equal(model.Rating, projectedSummary.Rating);
        Assert.Equal(model.ReleaseYear, projectedSummary.ReleaseYear);
    }
    
    [Fact]
    public void ToDto_Should_Throw_When_Model_Is_Null()
    {
        Anime model = null;

        var ex = Record.Exception(() => _mapper.MapToDto(model));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void ToModel_Should_Throw_When_Dto_Is_Null()
    {
        AnimeDto dto = null;
        var ex = Record.Exception(() => _mapper.MapToEntity(dto));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }
    
    [Fact]
    public void ToDto_Should_Map_Empty_Collections_Correctly()
    {
        var model = new Anime
        {
            AnimeGenres = new List<AnimeGenre>(),
            AnimeLicensors = new List<AnimeLicensor>(),
            AnimeProducers = new List<AnimeProducer>()
        };

        var dto = _mapper.MapToDto(model);
    
        Assert.Empty(dto.Genres);
        Assert.Empty(dto.Licensors);
        Assert.Empty(dto.Producers);
    }

}