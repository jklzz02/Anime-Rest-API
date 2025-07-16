using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Core.Abstractions.Business.Validators;

public interface IAnimeValidator : IValidator<AnimeDto>
{
}