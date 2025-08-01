using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Abstractions.Dto;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public static class BaseMapper
{
    public static TDto MapTo<TDto>(this IBaseModel model)
        where TDto : class, IBaseDto, new()
    {
        return new TDto
        {
            Id = model.Id,
            Name = model.Name
        };
    }
    
    public static TModel MapTo<TModel>(this IBaseDto dto)
        where TModel : class, IBaseModel, new()
    {
        return new TModel
        {
            Id = dto.Id.GetValueOrDefault(),
            Name = dto.Name ?? string.Empty
        };
    }

    public static IEnumerable<TDto> MapTo<TDto>(this IEnumerable<IBaseModel> models)
        where TDto : class, IBaseDto, new()
    {
        return models.Select(m => m.MapTo<TDto>());
    }

    public static IEnumerable<TModel> MapTo<TModel>(this IEnumerable<IBaseDto> dto)
        where TModel : class, IBaseModel, new()
    {
        return dto.Select(d => d.MapTo<TModel>());
    }
}