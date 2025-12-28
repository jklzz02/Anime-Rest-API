using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreHelper _helper;
    private readonly ICachingService _cache;

    public GenreController(IGenreHelper helper, ICachingService cache)
    {
        _helper = helper;
        _cache = cache;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var genres = await
            _cache
                .GetOrCreateAsync(
                    () => _helper.GetAllAsync(),
                    Constants.Cache.MinCachedItemSize);
        
        return Ok(genres);
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id)
    {
        var genre = await _helper.GetByIdAsync(id);

        if (genre is null) 
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNameAsync(
        [FromRoute, MaxLength(Constants.MaxTextQueryLength)] string name)
    {
        var genre = await _helper.GetByNameAsync(name);
        
        if (!genre.Any()) 
        {
            return NotFound();
        }
        
        return Ok(genre);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] GenreDto genre)
    {
        var result = await _helper.CreateAsync(genre);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] GenreDto genre)
    {
        if (string.IsNullOrEmpty(genre.Name))
        {
            return BadRequest();
        }
        
        var result = await _helper.UpdateAsync(genre);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var result = await _helper.DeleteAsync(id);
        
        if (!result) 
        {
            return NotFound();
        }

        return NoContent();
    }
}

