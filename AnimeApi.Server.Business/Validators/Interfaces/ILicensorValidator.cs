using AnimeApi.Server.Business.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

public interface ILicensorValidator : IValidator<LicensorDto>
{
}