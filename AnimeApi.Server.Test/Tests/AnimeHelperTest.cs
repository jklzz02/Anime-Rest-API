using System.Linq.Expressions;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Test.Generators;
using FluentValidation.Results;
using Moq;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using FluentValidation;

namespace AnimeApi.Server.Test.Tests;

public class AnimeHelperTest
{
    private readonly Mock<IAnimeRepository> _repositoryMock;
    private readonly Mock<IValidator<AnimeDto>> _validatorMock;

    public AnimeHelperTest()
    {
        _repositoryMock = new Mock<IAnimeRepository>();
        _validatorMock = new Mock<IValidator<AnimeDto>>();
    }

    [Fact]
    public async Task GetAll_Should_Return_Empty_Dto_List()
    {
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Anime>());
        
        var result = await service.GetAllAsync();
        Assert.IsAssignableFrom<IEnumerable<AnimeDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAll_Should_Return_Valid_Dto_List()
    {
        var animeList = AnimeGenerator.GetMockAnimeList();
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
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
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        var validationFailure = new ValidationFailure("test", "test error message");
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { validationFailure }));
        
        var result = await service.CreateAsync(new AnimeDto());
        Assert.Null(result);
        Assert.NotNull(service.ErrorMessages);
        Assert.Single(service.ErrorMessages);
        Assert.Equal(validationFailure.ErrorMessage, service.ErrorMessages[validationFailure.PropertyName]);
    }

    [Fact]
    public async Task Should_Return_Entity_When_Validation_Succeeds()
    {
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Anime>()))
            .ReturnsAsync(new Anime());
        
        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(validationResult);
        
        var result = await service.CreateAsync(new AnimeDto());
        Assert.True(result is AnimeDto);
        Assert.NotNull(service.ErrorMessages);
        Assert.Empty(service.ErrorMessages);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(36)]
    [InlineData(24)]
    public async Task GetById_Should_Return_AnimeDto_With_Correct_Id(int animeId)
    {
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Anime {Id = animeId });
        
        var result = await service.GetByIdAsync(animeId);
        Assert.NotNull(result);
        Assert.Equal(animeId, result.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetById_Should_Return_Null_For_Invalid_Id(int invalidAnimeId)
    {
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Anime?)null);
        
        var result = await service.GetByIdAsync(invalidAnimeId);
        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("TeSt")]
    public async Task Search_Should_Return_AnimeDto_With_Correct_Title(string title)
    {
        var animeList = AnimeGenerator.GetMockAnimeList();

        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        
        _repositoryMock
            .Setup(r => r.GetByConditionAsync(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<IEnumerable<Expression<Func<Anime, bool>>>>()))
            .ReturnsAsync((int page, int size, IEnumerable<Expression<Func<Anime, bool>>> filters) =>
            {
                var query = animeList.AsQueryable();
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }

                var entities = query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();

                return new PaginatedResult<Anime>(entities, page, size);
            });
        
        var parameters = new AnimeSearchParameters { Name = title };
        var result = await service.SearchAsync(parameters, 1);

        Assert.NotNull(result);
        Assert.True(result.Items.All(a => a?.Name?.Contains(title) ?? false));
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoTestData), MemberType = typeof(AnimeGenerator))]
    public async Task Create_Should_Return_Correct_Entity(AnimeDto animeDto)
    {
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Anime>()))
            .ReturnsAsync((Anime entity) => entity);
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var result = await service.CreateAsync(animeDto);
        Assert.NotNull(result);
        Assert.Empty(service.ErrorMessages);
        Assert.Equal(animeDto.Id, result.Id);
        Assert.Equal(animeDto.Name, result.Name);
    }

    [Theory]
    [MemberData(nameof(AnimeGenerator.GetAnimeDtoToAnimeTestData), MemberType = typeof(AnimeGenerator))]
    public async Task Create_Should_Add_Correct_Entity(AnimeDto animeDto, Anime expectedModel)
    {
        List<Anime> anime = [];
        
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Anime>()))
            .ReturnsAsync((Anime entity) =>
            {
                anime.Add(entity);
                return entity;
            });
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        var result = await service.CreateAsync(animeDto);
        Assert.NotNull(result);
        Assert.Empty(service.ErrorMessages);
        Assert.Equal(expectedModel.Id, anime[0].Id);
        Assert.Equal(expectedModel.Name, anime[0].Name);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(6, null)]
    [InlineData(-1, null)]
    [InlineData(-0, null)]
    public async Task Update_Should_Return_Entity_With_Correct_Id(int id, int? expectedId)
    {
        var anime = AnimeGenerator.GetMockAnimeList();
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Anime>()))
            .ReturnsAsync((Anime entity) =>
            {
                if (anime.Any(a => a.Id == entity.Id))
                {
                    return anime.FirstOrDefault(a => a.Id == entity.Id);
                }
                return null;
            });
        
        _validatorMock
            .Setup(a => a.ValidateAsync(It.IsAny<AnimeDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        var result =  await service.UpdateAsync(new AnimeDto { Id = id });
        Assert.Equal(expectedId, result?.Id);
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
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((int passedId) =>
            {
                return anime.Any(a => a.Id == passedId);
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
        var service = new AnimeHelper(_repositoryMock.Object, _validatorMock.Object);
        _repositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((int passedId) =>
            {
                var toBeRemoved = anime.First(a => a.Id == passedId);
                anime.Remove(toBeRemoved);
                return true;
            });
        
        var result = await service.DeleteAsync(validId);
        Assert.True(result);
        Assert.DoesNotContain(anime, a => a.Id == validId);
    }
}