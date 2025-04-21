using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validator.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validator;

public class GenreValidator : AbstractValidator<GenreDto>, IGenreValidator
{
    public GenreValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}