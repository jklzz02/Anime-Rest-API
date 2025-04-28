using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class LicensorValidator : AbstractValidator<LicensorDto>, ILicensorValidator
{
    public LicensorValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}