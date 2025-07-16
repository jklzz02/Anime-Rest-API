using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class AnimeValidator : AbstractValidator<AnimeDto>, IAnimeValidator
{
    public AnimeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("The 'title' field cannot be empty.")
            .MaximumLength(255)
                .WithMessage("The 'title' field cannot be longer than 255 characters.");
        
        RuleFor(x => x.EnglishName)
            .NotEmpty()
                .WithMessage("The 'english_title' field cannot be empty.")
            .MaximumLength(255)
                .WithMessage("The 'english_title' field cannot be longer than 255 characters.");
        
        RuleFor(x => x.OtherName)
            .NotEmpty()
                .WithMessage("The 'other_title' field cannot be empty.")
            .MaximumLength(255)
                .WithMessage("The 'other_title' field cannot be longer than 255 characters.");
        
        RuleFor(x => x.Synopsis)
            .NotEmpty()
                .WithMessage("The 'synopsis' cannot be empty.")
            .MaximumLength(5000)
                .WithMessage("The 'synopsis' cannot be longer than 5000 characters.'");
        
        RuleFor(x => x.ImageUrl)
            .NotEmpty()
                .WithMessage("The 'image_url' cannot be empty.")
            .MaximumLength(255)
                .WithMessage("The 'image_url' cannot be longer than 255 characters.'");
        
        RuleFor(x => x.Episodes)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("'episodes' must be greater than or equal to 0.");
        
        RuleFor(x => x.Duration)
            .NotEmpty()
                .WithMessage("The 'duration' cannot be empty.")
            .MaximumLength(255)
                .WithMessage("The 'duration' cannot be longer than 255 characters.");

        RuleFor(x => x.ReleaseYear)
            .GreaterThanOrEqualTo(1950)
            .WithMessage("The 'release_year' must be greater than or equal to 1950.'");
        
        RuleFor(x => x.Status)
            .NotEmpty()
                .WithMessage("The 'status' cannot be empty.'")
            .MaximumLength(50)
                .WithMessage("The 'status' cannot be longer than 50 characters.'");

        RuleFor(x => x.Rating)
            .NotEmpty()
                .WithMessage("The 'rating' cannot be empty.")
            .MaximumLength(100)
                .WithMessage("The 'rating' cannot be longer than 100 characters.");
        
        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The 'score' must be greater than or equal to 0.");
        
        RuleFor(x => x.Background)
            .MaximumLength(1000)
            .WithMessage("The 'background' cannot be longer than 1000 characters.");
        
        RuleFor(x => x.TrailerUrl)
            .MaximumLength(255)
            .WithMessage("The 'trailer_url' cannot be longer than 255 characters.");
        
        RuleFor(x => x.TrailerEmbedUrl)
            .MaximumLength(255)
            .WithMessage("The 'trailer_embed_url' cannot be longer than 255 characters.");
        
        RuleFor(x => x.TrailerImageUrl)
            .MaximumLength(255)
            .WithMessage("The 'trailer_image_url' cannot be longer than 255 characters.");
    }
}