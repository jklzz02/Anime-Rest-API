using AnimeApi.Server.Core.Objects.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

public interface IAnimeValidator : IValidator<AnimeDto>
{
}