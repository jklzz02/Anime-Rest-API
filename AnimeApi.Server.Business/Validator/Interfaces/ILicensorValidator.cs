using AnimeApi.Server.Business.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validator.Interfaces;

public interface ILicensorValidator : IValidator<LicensorDto>
{
}