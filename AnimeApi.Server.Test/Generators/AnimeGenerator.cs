using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Test.Generators;

/// <summary>
/// Provides utility methods for generating mock data for Anime-related testing.
/// </summary>
/// <remarks>
/// This static class is intended to supply test data in the form of DTOs and domain models,
/// as well as data mapping examples for validation in unit tests.
/// </remarks>
public static class AnimeGenerator
{
    /// <summary>
    /// Provides test data for mapping between <see cref="AnimeDto"/> and <see cref="Anime"/>.
    /// </summary>
    /// <returns>
    /// A collection of object arrays, where each array contains a <see cref="AnimeDto"/> and its expected <see cref="Anime"/> model.
    /// </returns>
    public static IEnumerable<object[]> GetAnimeDtoToAnimeTestData()
    {
        var dtos = GetMockAnimeDtoList();
        var models = GetMockAnimeList();

        for (int i = 0; i < dtos.Count; i++)
        {
            yield return [dtos[i], models[i]];
        }
    }

    /// <summary>
    /// Provides test data for validating <see cref="AnimeDto"/> mapping and operations.
    /// </summary>
    /// <returns>
    /// A collection of object arrays, where each array contains a single <see cref="AnimeDto"/> entry for testing purposes.
    /// </returns>
    public static IEnumerable<object[]> GetAnimeDtoTestData()
    {
        var dtos = GetMockAnimeDtoList();

        foreach (var dto in dtos)
        {
            yield return [dto];
        }
    }
    
    public static List<Anime> GetMockAnimeList()
    {
        return 
        [
            new Anime
            {
                Id = 1,
                Name = "Attack on Titan",
                EnglishName = "Attack on Titan",
                OtherName = "Shingeki no Kyojin",
                Synopsis = "Titans attack humanity.",
                ImageUrl = "url1",
                Type = new Type {Id = 1},
                TypeId = 1,
                Episodes = 25,
                Duration = "24 min",
                Source = new Source {Id = 1},
                SourceId = 1,
                ReleaseYear = 2020,
                StartedAiring = new DateTime(2020, 1, 10),
                FinishedAiring = new DateTime(2020, 6, 20),
                Rating = "R",
                Studio = "Wit Studio",
                Score = 9.2m,
                Status = "Finished",
                TrailerUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                TrailerImageUrl = "https://i.ytimg.com/vi/dQw4w9WgXcQ/maxresdefault.jpg",
                TrailerEmbedUrl = "https://www.youtube.com/embed/dQw4w9WgXcQ",
                AnimeGenres = { new AnimeGenre { GenreId = 1, Genre = new Genre { Id = 1, Name = "Action" } } },
                AnimeLicensors = { new AnimeLicensor { LicensorId = 1, Licensor = new Licensor { Id = 1, Name = "test" } } },
                AnimeProducers = { new AnimeProducer { ProducerId = 1, Producer = new Producer { Id = 1, Name = "test" } } }
            },
            new Anime
            {
                Id = 2,
                Name = "Spirited Away",
                EnglishName = "Spirited Away",
                OtherName = "Sen to Chihiro",
                Synopsis = "Girl in a spirit world.",
                ImageUrl = "url2",
                Type = new Type {Id = 1},
                TypeId = 1,
                Episodes = 1,
                Duration = "125 min",
                Source = new Source {Id = 1},
                SourceId = 1,
                ReleaseYear = 2001,
                StartedAiring = new DateTime(2001, 7, 20),
                FinishedAiring = new DateTime(2001, 7, 20),
                Rating = "PG",
                Studio = "Ghibli",
                Score = 9.5m,
                Status = "Finished",
                TrailerUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                TrailerImageUrl = "https://i.ytimg.com/vi/dQw4w9WgXcQ/maxresdefault.jpg",
                TrailerEmbedUrl = "https://www.youtube.com/embed/dQw4w9WgXcQ",
                AnimeGenres = { new AnimeGenre { GenreId = 2, Genre = new Genre { Id = 2, Name = "Fantasy" } } },
                AnimeLicensors = { new AnimeLicensor { LicensorId = 2, Licensor = new Licensor { Id = 2, Name = "Disney" } } },
                AnimeProducers = { new AnimeProducer { ProducerId = 2, Producer = new Producer { Id = 2, Name = "Ghibli" } } }
            },
            new Anime
            {
                Id = 3,
                Name = "Naruto",
                EnglishName = "Naruto",
                OtherName = "N/A",
                Synopsis = "Ninja boy’s journey.",
                ImageUrl = "url3",
                Type = new Type {Id = 1},
                TypeId = 1,
                Episodes = 220,
                Duration = "23 min",
                Source = new Source {Id = 1},
                SourceId = 1,
                ReleaseYear = 2002,
                StartedAiring = new DateTime(2002, 10, 3),
                FinishedAiring = new DateTime(2007, 2, 8),
                Rating = "PG-13",
                Studio = "Pierrot",
                Score = 8.2m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                AnimeGenres = { new AnimeGenre { GenreId = 1, Genre = new Genre { Id = 1, Name = "Action" } }, new AnimeGenre { GenreId = 3, Genre = new Genre { Id = 3, Name = "Adventure" } } },
                AnimeLicensors = { new AnimeLicensor { LicensorId = 3, Licensor = new Licensor { Id = 3, Name = "Viz Media" } } },
                AnimeProducers = { new AnimeProducer { ProducerId = 3, Producer = new Producer { Id = 3, Name = "TV Tokyo" } } }
            },
            new Anime
            {
                Id = 4,
                Name = "Made in Abyss",
                EnglishName = "Made in Abyss",
                OtherName = "N/A",
                Synopsis = "Girl explores a deadly abyss.",
                ImageUrl = "url4",
                Type = new Type {Id = 1},
                TypeId = 1,
                Episodes = 13,
                Duration = "25 min",
                Source = new Source {Id = 1},
                SourceId = 1,
                ReleaseYear = 2017,
                StartedAiring = new DateTime(2017, 7, 7),
                FinishedAiring = new DateTime(2017, 9, 29),
                Rating = "R",
                Studio = "Kinema Citrus",
                Score = 8.9m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                AnimeGenres = { new AnimeGenre { GenreId = 2, Genre = new Genre { Id = 2, Name = "Fantasy" } }, new AnimeGenre { GenreId = 3, Genre = new Genre { Id = 3, Name = "Adventure" } } },
                AnimeLicensors = { new AnimeLicensor { LicensorId = 1, Licensor = new Licensor { Id = 1, Name = "test" } } },
                AnimeProducers = { new AnimeProducer { ProducerId = 4, Producer = new Producer { Id = 4, Name = "Kadokawa" } } }
            },
            new Anime
            {
                Id = 5,
                Name = "Random Anime",
                EnglishName = "Random",
                OtherName = "N/A",
                Synopsis = "Just filler.",
                ImageUrl = "url5",
                Type = new Type {Id = 1},
                TypeId = 1,
                Episodes = 12,
                Duration = "22 min",
                Source = new Source {Id = 1},
                SourceId = 1,
                ReleaseYear = 1985,
                StartedAiring = new DateTime(1985, 4, 1),
                FinishedAiring = new DateTime(1985, 7, 15),
                Rating = "PG",
                Studio = "OldStudio",
                Score = 7.0m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                AnimeGenres = { new AnimeGenre { GenreId = 4, Genre = new Genre { Id = 4, Name = "Comedy" } } },
                AnimeLicensors = { new AnimeLicensor { LicensorId = 4, Licensor = new Licensor { Id = 4, Name = "ObscureLicensor" } } },
                AnimeProducers = { new AnimeProducer { ProducerId = 5, Producer = new Producer { Id = 5, Name = "RetroStudio" } } }
            }
        ];
    }

    public static IEnumerable<object[]> GetAnimeTestData()
    {
        IEnumerable<Anime> anime = GetMockAnimeList();
        foreach (var item in anime)
        {
            yield return [item];
        }
    }
    
    public static List<AnimeDto> GetMockAnimeDtoList()
    {
        return 
        [
            new AnimeDto
            {
                Id = 1,
                Name = "Attack on Titan",
                EnglishName = "Attack on Titan",
                OtherName = "Shingeki no Kyojin",
                Synopsis = "Titans attack humanity.",
                ImageUrl = "url1",
                Type = new TypeDto {Id = 1},
                Episodes = 25,
                Duration = "24 min",
                Source = new SourceDto {Id = 1},
                ReleaseYear = 2020,
                StartedAiring = new DateTime(2020, 1, 10),
                FinishedAiring = new DateTime(2020, 6, 20),
                Rating = "R",
                Studio = "Wit Studio",
                Score = 9.2m,
                Status = "Finished",
                TrailerUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                TrailerImageUrl = "https://i.ytimg.com/vi/dQw4w9WgXcQ/maxresdefault.jpg",
                TrailerEmbedUrl = "https://www.youtube.com/embed/dQw4w9WgXcQ",
                Genres = [ new GenreDto { Id = 1, Name = "Action"} ],
                Licensors = [ new LicensorDto { Id = 1, Name = "test" } ],
                Producers = [ new ProducerDto { Id = 1, Name = "test" } ]
            },
            new AnimeDto
            {
                Id = 2,
                Name = "Spirited Away",
                EnglishName = "Spirited Away",
                OtherName = "Sen to Chihiro",
                Synopsis = "Girl in a spirit world.",
                ImageUrl = "url2",
                Type = new TypeDto {Id = 1},
                Episodes = 1,
                Duration = "125 min",
                Source = new SourceDto {Id = 1},
                ReleaseYear = 2001,
                StartedAiring = new DateTime(2001, 7, 20),
                FinishedAiring = new DateTime(2001, 7, 20),
                Rating = "PG",
                Studio = "Ghibli",
                Score = 9.5m,
                Status = "Finished",
                TrailerUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                TrailerImageUrl = "https://i.ytimg.com/vi/dQw4w9WgXcQ/maxresdefault.jpg",
                TrailerEmbedUrl = "https://www.youtube.com/embed/dQw4w9WgXcQ",
                Genres = [ new GenreDto { Id = 2, Name = "Fantasy" } ],
                Licensors = [ new LicensorDto { Id = 2, Name = "Disney" } ],
                Producers = [ new ProducerDto { Id = 2, Name = "Ghibli" } ]
            },
            new AnimeDto
            {
                Id = 3,
                Name = "Naruto",
                EnglishName = "Naruto",
                OtherName = "N/A",
                Synopsis = "Ninja boy’s journey.",
                ImageUrl = "url3",
                Type = new TypeDto {Id = 1},
                Episodes = 220,
                Duration = "23 min",
                Source = new SourceDto {Id = 1},
                ReleaseYear = 2002,
                StartedAiring = new DateTime(2002, 10, 3),
                FinishedAiring = new DateTime(2007, 2, 8),
                Rating = "PG-13",
                Studio = "Pierrot",
                Score = 8.2m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                Genres = [ new GenreDto { Id = 1, Name = "Action" } ,  new GenreDto { Id = 3, Name = "Adventure" } ],
                Licensors = [ new LicensorDto { Id = 3, Name = "Viz Media" } ],
                Producers = [ new ProducerDto { Id = 3, Name = "TV Tokyo" } ]
            },
            new AnimeDto
            {
                Id = 4,
                Name = "Made in Abyss",
                EnglishName = "Made in Abyss",
                OtherName = "N/A",
                Synopsis = "Girl explores a deadly abyss.",
                ImageUrl = "url4",
                Type = new TypeDto { Id = 1},
                Episodes = 13,
                Duration = "25 min",
                Source = new SourceDto { Id = 1},
                ReleaseYear = 2017,
                StartedAiring = new DateTime(2017, 7, 7),
                FinishedAiring = new DateTime(2017, 9, 29),
                Rating = "R",
                Studio = "Kinema Citrus",
                Score = 8.9m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                Genres = [ new GenreDto { Id = 2, Name = "Fantasy" } , new GenreDto { Id = 3, Name = "Adventure" } ],
                Licensors = [ new LicensorDto { Id = 1, Name = "test" } ],
                Producers = [ new ProducerDto { Id = 4, Name = "Kadokawa" } ]
            },
            new AnimeDto
            {
                Id = 5,
                Name = "Random Anime",
                EnglishName = "Random",
                OtherName = "N/A",
                Synopsis = "Just filler.",
                ImageUrl = "url5",
                Type = new TypeDto { Id = 1},
                Episodes = 12,
                Duration = "22 min",
                Source = new SourceDto { Id = 1},
                ReleaseYear = 1985,
                StartedAiring = new DateTime(1985, 4, 1),
                FinishedAiring = new DateTime(1985, 7, 15),
                Rating = "PG",
                Studio = "OldStudio",
                Score = 7.0m,
                Status = "Finished",
                TrailerUrl = "",
                TrailerEmbedUrl = "",
                TrailerImageUrl = "",
                Genres = [ new GenreDto { Id = 4, Name = "Comedy" } ],
                Licensors = [ new LicensorDto { Id = 4, Name = "ObscureLicensor" } ],
                Producers = [ new ProducerDto { Id = 5, Name = "RetroStudio" } ]
            }
        ];
    }
}
