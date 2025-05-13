using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Business.Services.Interfaces;
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
        _cache.DefaultExpiration = TimeSpan.FromMinutes(5);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page)
    {
        var anime = await _cache
            .GetOrCreateAsync($"anime-page-{page}", () => _helper.GetAllAsync(page));

        if (anime is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if(!anime.Any()) return NotFound();
        
        return Ok(anime);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var anime = await _cache
            .GetOrCreateAsync($"anime-id-{id}",() => _helper.GetByIdAsync(id));
        
        if (anime is null) return NotFound();
        
        return Ok(anime);
    }
    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByParametersAsync([FromQuery] AnimeSearchParameters parameters, int page)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                $"anime-page{page}-search-{JsonConvert.SerializeObject(parameters)}",
                () => _helper.SearchAsync(parameters, page));
        
        if (anime is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if(!anime.Any()) return NotFound();
        
        return Ok(anime);
    }

    [HttpGet]
    [Route("year/{year:int:min(1900)}/page/{page:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByReleaseYearAsync([FromRoute] int year, int page)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                $"anime-year-{year}-page-{page}",
                () => _helper.GetByReleaseYearAsync(year, page));
        
        if (anime is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        if (!anime.Any()) return NotFound();
        return Ok(anime);
    }

    [HttpGet]
    [Route("episodes/{episodes:int:min(1)}/page/{page:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByEpisodes([FromRoute] int episodes, int page)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                $"anime-episodes-{episodes}-page{page}",
                () => _helper.GetByEpisodesAsync(episodes, page));
        
        if (anime is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if(!anime.Any()) return NotFound();
        return Ok(anime);
    }

    [HttpGet]
    [Route("title/{title}/page/{page:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByTitleAsync([FromRoute] string title, int page)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                $"anime-title-{title}-page-{page}",
                () => _helper.GetByNameAsync(title, page));
        
        if (anime is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        if (!anime.Any()) return NotFound();
        return Ok(anime);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);
        if(!result) return NotFound();
        
        return Ok();
    }
}