using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class AnimeValidator : AbstractValidator<AnimeDto>, IAnimeValidator
{
    private readonly string[] _animeTypes = ["TV", "Movie", "OVA", "ONA", "Special", "Music", "UNKNOWN" ];
    public AnimeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.EnglishName)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.OtherName)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.Synopsis)
            .NotEmpty()
            .MaximumLength(5000);
        
        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(x => _animeTypes.Contains(x));
        
        RuleFor(x => x.Episodes)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Duration)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.Source)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ReleaseYear)
            .GreaterThanOrEqualTo(1950);
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Rating)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0);

    }
}