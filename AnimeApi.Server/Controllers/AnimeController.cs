using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimeController : ControllerBase
{
    private readonly IAnimeHelper _helper;
    private readonly ICachingService _cache;
    public AnimeController(IAnimeHelper helper, ICachingService cachingService)
    {
        _helper = helper;
        _cache = cachingService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync([FromQuery] int page, int size = Constants.Pagination.MaxPageSize)
    {
        var result = await _cache
            .GetOrCreateAsync(
                $"anime-page{page}-size{size}",
                () => _helper.GetAllAsync(page, size));
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if (!result.HasItems) 
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var anime = await _cache
            .GetOrCreateAsync($"anime-id-{id}",() => _helper.GetByIdAsync(id));
        
        if (anime is null)
        {
            return NotFound();
        }

        return Ok(anime);
    }
    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByParametersAsync(
        [FromQuery] AnimeSearchParameters parameters,
        int page,
        int size = Constants.Pagination.MaxPageSize)
    {
        var result = await _cache
            .GetOrCreateAsync(
                $"anime-page{page}-size{size}-search-{JsonConvert.SerializeObject(parameters)}",
                () => _helper.SearchAsync(parameters, page, size),
                Constants.Cache.MaxCachedItemSize);
        
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if(!result.HasItems) 
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("year/{year:int:min(1900)}/page/{page:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReleaseYearAsync([FromRoute] int year, int page, int size = Constants.Pagination.MaxPageSize)
    {
        var result = await _cache
            .GetOrCreateAsync(
                $"anime-year-{year}-page-{page}-size{size}",
                () => _helper.GetByReleaseYearAsync(year, page, size));
        
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        if (!result.HasItems) 
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("episodes/{episodes:int:min(1)}/page/{page:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEpisodes([FromRoute] int episodes, int page, int size = Constants.Pagination.MaxPageSize)
    {
        var result = await _cache
            .GetOrCreateAsync(
                $"anime-episodes-{episodes}-page{page}-size{size}",
                () => _helper.GetByEpisodesAsync(episodes, page, size));
        
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        if (!result.HasItems) 
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet]
    [Route("title/{title}/page/{page:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTitleAsync([FromRoute] string title, int page, int size = Constants.Pagination.MaxPageSize)
    {
        var result = await _cache
            .GetOrCreateAsync(
                $"anime-title-{title}-page-{page}-size{size}",
                () => _helper.GetByNameAsync(title, page, size));
        
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if (!result.HasItems) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.CreateAsync(anime);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        };

        return Ok(result);
    }
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartialAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.UpdateAsync(anime);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        return Ok(result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.UpdateAsync(anime);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]   
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);

        if (!result) 
        {
            return NotFound();
        }
        
        return NoContent();
    }
}