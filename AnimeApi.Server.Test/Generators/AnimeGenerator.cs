using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Test.Generators;

public static class AnimeGenerator
{
    public static IEnumerable<object[]> GetAnimeDtoToAnimeTestData()
    {
        var dtos = GetMockAnimeDtoList();
        var models = GetMockAnimeList();

        for (int i = 0; i < dtos.Count; i++)
        {
            yield return [dtos[i], models[i]];
        }
    }

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
                English_Name = "Attack on Titan",
                Other_Name = "Shingeki no Kyojin",
                Synopsis = "Titans attack humanity.",
                Image_URL = "url1",
                Type = "TV",
                Episodes = 25,
                Duration = "24 min",
                Source = "Manga",
                Release_Year = 2020,
                Started_Airing = new DateOnly(2020, 1, 10),
                Finished_Airing = new DateOnly(2020, 6, 20),
                Rating = "R",
                Studio = "Wit Studio",
                Score = 90,
                Status = "Finished",
                Anime_Genres = { new Anime_Genre { GenreId = 1, Genre = new Genre { Id = 1, Name = "Action" } } },
                Anime_Licensors = { new Anime_Licensor { LicensorId = 1, Licensor = new Licensor { Id = 1, Name = "test" } } },
                Anime_Producers = { new Anime_Producer { ProducerId = 1, Producer = new Producer { Id = 1, Name = "test" } } }
            },
            new Anime
            {
                Id = 2,
                Name = "Spirited Away",
                English_Name = "Spirited Away",
                Other_Name = "Sen to Chihiro",
                Synopsis = "Girl in a spirit world.",
                Image_URL = "url2",
                Type = "Movie",
                Episodes = 1,
                Duration = "125 min",
                Source = "Original",
                Release_Year = 2001,
                Started_Airing = new DateOnly(2001, 7, 20),
                Finished_Airing = new DateOnly(2001, 7, 20),
                Rating = "PG",
                Studio = "Ghibli",
                Score = 95,
                Status = "Finished",
                Anime_Genres = { new Anime_Genre { GenreId = 2, Genre = new Genre { Id = 2, Name = "Fantasy" } } },
                Anime_Licensors = { new Anime_Licensor { LicensorId = 2, Licensor = new Licensor { Id = 2, Name = "Disney" } } },
                Anime_Producers = { new Anime_Producer { ProducerId = 2, Producer = new Producer { Id = 2, Name = "Ghibli" } } }
            },
            new Anime
            {
                Id = 3,
                Name = "Naruto",
                English_Name = "Naruto",
                Other_Name = "N/A",
                Synopsis = "Ninja boy’s journey.",
                Image_URL = "url3",
                Type = "TV",
                Episodes = 220,
                Duration = "23 min",
                Source = "Manga",
                Release_Year = 2002,
                Started_Airing = new DateOnly(2002, 10, 3),
                Finished_Airing = new DateOnly(2007, 2, 8),
                Rating = "PG-13",
                Studio = "Pierrot",
                Score = 82,
                Status = "Finished",
                Anime_Genres = { new Anime_Genre { GenreId = 1, Genre = new Genre { Id = 1, Name = "Action" } }, new Anime_Genre { GenreId = 3, Genre = new Genre { Id = 3, Name = "Adventure" } } },
                Anime_Licensors = { new Anime_Licensor { LicensorId = 3, Licensor = new Licensor { Id = 3, Name = "Viz Media" } } },
                Anime_Producers = { new Anime_Producer { ProducerId = 3, Producer = new Producer { Id = 3, Name = "TV Tokyo" } } }
            },
            new Anime
            {
                Id = 4,
                Name = "Made in Abyss",
                English_Name = "Made in Abyss",
                Other_Name = "N/A",
                Synopsis = "Girl explores a deadly abyss.",
                Image_URL = "url4",
                Type = "TV",
                Episodes = 13,
                Duration = "25 min",
                Source = "Web Manga",
                Release_Year = 2017,
                Started_Airing = new DateOnly(2017, 7, 7),
                Finished_Airing = new DateOnly(2017, 9, 29),
                Rating = "R",
                Studio = "Kinema Citrus",
                Score = 89,
                Status = "Finished",
                Anime_Genres = { new Anime_Genre { GenreId = 2, Genre = new Genre { Id = 2, Name = "Fantasy" } }, new Anime_Genre { GenreId = 3, Genre = new Genre { Id = 3, Name = "Adventure" } } },
                Anime_Licensors = { new Anime_Licensor { LicensorId = 1, Licensor = new Licensor { Id = 1, Name = "test" } } },
                Anime_Producers = { new Anime_Producer { ProducerId = 4, Producer = new Producer { Id = 4, Name = "Kadokawa" } } }
            },
            new Anime
            {
                Id = 5,
                Name = "Random Anime",
                English_Name = "Random",
                Other_Name = "N/A",
                Synopsis = "Just filler.",
                Image_URL = "url5",
                Type = "TV",
                Episodes = 12,
                Duration = "22 min",
                Source = "Light Novel",
                Release_Year = 1985,
                Started_Airing = new DateOnly(1985, 4, 1),
                Finished_Airing = new DateOnly(1985, 7, 15),
                Rating = "PG",
                Studio = "OldStudio",
                Score = 70,
                Status = "Finished",
                Anime_Genres = { new Anime_Genre { GenreId = 4, Genre = new Genre { Id = 4, Name = "Comedy" } } },
                Anime_Licensors = { new Anime_Licensor { LicensorId = 4, Licensor = new Licensor { Id = 4, Name = "ObscureLicensor" } } },
                Anime_Producers = { new Anime_Producer { ProducerId = 5, Producer = new Producer { Id = 5, Name = "RetroStudio" } } }
            }
        ];
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
                Type = "TV",
                Episodes = 25,
                Duration = "24 min",
                Source = "Manga",
                ReleaseYear = 2020,
                StartedAiring = new DateOnly(2020, 1, 10),
                FinishedAiring = new DateOnly(2020, 6, 20),
                Rating = "R",
                Studio = "Wit Studio",
                Score = 90,
                Status = "Finished",
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
                Type = "Movie",
                Episodes = 1,
                Duration = "125 min",
                Source = "Original",
                ReleaseYear = 2001,
                StartedAiring = new DateOnly(2001, 7, 20),
                FinishedAiring = new DateOnly(2001, 7, 20),
                Rating = "PG",
                Studio = "Ghibli",
                Score = 95,
                Status = "Finished",
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
                Type = "TV",
                Episodes = 220,
                Duration = "23 min",
                Source = "Manga",
                ReleaseYear = 2002,
                StartedAiring = new DateOnly(2002, 10, 3),
                FinishedAiring = new DateOnly(2007, 2, 8),
                Rating = "PG-13",
                Studio = "Pierrot",
                Score = 82,
                Status = "Finished",
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
                Type = "TV",
                Episodes = 13,
                Duration = "25 min",
                Source = "Web Manga",
                ReleaseYear = 2017,
                StartedAiring = new DateOnly(2017, 7, 7),
                FinishedAiring = new DateOnly(2017, 9, 29),
                Rating = "R",
                Studio = "Kinema Citrus",
                Score = 89,
                Status = "Finished",
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
                Type = "TV",
                Episodes = 12,
                Duration = "22 min",
                Source = "Light Novel",
                ReleaseYear = 1985,
                StartedAiring = new DateOnly(1985, 4, 1),
                FinishedAiring = new DateOnly(1985, 7, 15),
                Rating = "PG",
                Studio = "OldStudio",
                Score = 70,
                Status = "Finished",
                Genres = [ new GenreDto { Id = 4, Name = "Comedy" } ],
                Licensors = [ new LicensorDto { Id = 4, Name = "ObscureLicensor" } ],
                Producers = [ new ProducerDto { Id = 5, Name = "RetroStudio" } ]
            }
        ];
    }
}
