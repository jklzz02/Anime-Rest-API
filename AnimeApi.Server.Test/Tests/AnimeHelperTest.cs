using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Test.Generators;
using FluentValidation.Results;
using Moq;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using FluentValidation;

namespace AnimeApi.Server.Test.Tests;

public class AnimeHelperTest
{
    private readonly Mock<IRepository<Anime, AnimeDto>> _repositoryMock;
    private readonly Mock<IValidator<AnimeDto>> _validatorMock;
    private readonly Mock<IValidator<AnimeSearchParameters>> _searchParametersValidatorMock;

    public AnimeHelperTest()
    {
        _repositoryMock = new Mock<IRepository<Anime, AnimeDto>>();
        _validatorMock = new Mock<IValidator<AnimeDto>>();
        _searchParametersValidatorMock = new Mock<IValidator<AnimeSearchParameters>>();
        _searchParametersValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeSearchParameters>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
    }

    [Fact]
    public async Task GetAll_Should_Return_Empty_Dto_List()
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<AnimeDto>());
        
        var result = await service.GetAllAsync();
        Assert.IsAssignableFrom<IEnumerable<AnimeDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAll_Should_Return_Valid_Dto_List()
    {
        var animeList = AnimeGenerator.GetMockAnimeDtoList();
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);        
        
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(animeList);
        
        var result = await service.GetAllAsync();
        var resultList = result.ToList();
        Assert.IsAssignableFrom<IEnumerable<AnimeDto>>(result);
        Assert.Equal(animeList.Count, resultList.Count);

        for (int i = 0; i < animeList.Count(); i++)
        {
            Assert.Equal(animeList[i].Id, resultList[i].Id);
            Assert.Equal(animeList[i].Name, resultList[i].Name);
        }
        
    }

    [Fact]
    public async Task Should_Return_Null_When_Validation_Fails()
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);        
        
        var validationFailure = new ValidationFailure("test", "test error message");
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { validationFailure }));
        
        var result = await service.CreateAsync(new AnimeDto());
        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(validationFailure.ErrorMessage, result.Errors.First().Details);
    }

    [Fact]
    public async Task Should_Return_Entity_When_Validation_Succeeds()
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<AnimeDto>()))
            .ReturnsAsync(Result<AnimeDto>.Success(new AnimeDto()));
        
        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(validationResult);
        
        var result = await service.CreateAsync(new AnimeDto());
        Assert.IsType<AnimeDto>(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(36)]
    [InlineData(24)]
    public async Task GetById_Should_Return_AnimeDto_With_Correct_Id(int animeId)
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);        
        
        _repositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<ISpecification<Anime>>()))
            .ReturnsAsync(new AnimeDto {Id = animeId });
        
        var result = await service.GetByIdAsync(animeId);
        Assert.NotNull(result);
        Assert.Equal(animeId, result.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetById_Should_Return_Null_For_Invalid_Id(int invalidAnimeId)
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.FindFirstOrDefaultAsync(It.IsAny<ISpecification<Anime>>()))
            .ReturnsAsync((AnimeDto?)null);
        
        var result = await service.GetByIdAsync(invalidAnimeId);
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoTestData), MemberType = typeof(AnimeGenerator))]
    public async Task Create_Should_Return_Correct_Entity(AnimeDto animeDto)
    {
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<AnimeDto>()))
            .ReturnsAsync((AnimeDto entity) => Result<AnimeDto>.Success(entity));
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var result = await service.CreateAsync(animeDto);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(animeDto.Id, result.Data.Id);
        Assert.Equal(animeDto.Name, result.Data.Name);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public async Task Create_Should_Add_Correct_Entity(AnimeDto animeDto, Anime expectedModel)
    {
        List<AnimeDto> anime = [];
        
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<AnimeDto>()))
            .ReturnsAsync((AnimeDto entity) =>
            {
                anime.Add(entity);
                return Result<AnimeDto>.Success(anime.Last());
            });
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        var result = await service.CreateAsync(animeDto);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(expectedModel.Id, result.Data.Id);
        Assert.Equal(expectedModel.Name, result.Data.Name);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(6, false)]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    public async Task Delete_Should_Return_True_With_Correct_Id(int id, bool expected)
    {
        var anime = AnimeGenerator.GetMockAnimeList();
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<ISpecification<Anime>>()))
            .ReturnsAsync((ISpecification<Anime> spec) =>
            {
                return spec.Apply(anime.AsQueryable()).Any();
            });
        
        var result = await service.DeleteAsync(id);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Delete_Should_Remove_Targeted_Entity(int validId)
    {
        var anime = AnimeGenerator.GetMockAnimeList();
        var service = new AnimeHelper(
            _repositoryMock.Object,
            _validatorMock.Object,
            _searchParametersValidatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<ISpecification<Anime>>()))
            .ReturnsAsync((ISpecification<Anime> spec) =>
            {
                var toBeRemoved = spec.Apply(anime.AsQueryable()).First();
                anime.Remove(toBeRemoved);
                return true;
            });
        
        var result = await service.DeleteAsync(validId);
        Assert.True(result);
        Assert.DoesNotContain(anime, a => a.Id == validId);
    }
}