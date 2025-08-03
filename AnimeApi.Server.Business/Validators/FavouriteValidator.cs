using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class FavouriteValidator : AbstractValidator<FavouriteDto>
{
    public FavouriteValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .GreaterThan(0)
            .WithMessage("must be greater than 0");
        
        RuleFor(f => f.AnimeId)
            .NotEmpty()
            .WithMessage("cannot be empty")
            .GreaterThan(0)
            .WithMessage("must be greater than 0");
    }
}