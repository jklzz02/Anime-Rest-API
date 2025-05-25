using AnimeApi.Server.Business.Dto;
using FluentValidation;

namespace AnimeApi.Server.Business.Validators.Interfaces;

public interface ISourceValidator : IBaseValidator<SourceDto, ISourceValidator>
{
}