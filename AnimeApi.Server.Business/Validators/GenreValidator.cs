using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class GenreValidator : AbstractValidator<GenreDto>, IGenreValidator
{
    public GenreValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The name must cannot be longer than 50 characters");
    }
}