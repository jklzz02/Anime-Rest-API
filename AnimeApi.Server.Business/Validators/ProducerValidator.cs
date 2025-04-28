using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Validators.Interfaces;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators;

public class ProducerValidator : AbstractValidator<ProducerDto>, IProducerValidator
{
    public ProducerValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}