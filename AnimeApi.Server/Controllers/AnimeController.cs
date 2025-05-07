using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimeController : ControllerBase
{
    private readonly IAnimeHelper _helper;
    public AnimeController(IAnimeHelper helper)
    {
        _helper = helper;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var anime = await _helper.GetAllAsync();
        return Ok(anime);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var anime = await _helper.GetByIdAsync(id);
        if (anime is null) return NotFound();
        
        return Ok(anime);
    }    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByParametersAsync([FromQuery] AnimeSearchParameters parameters)
    {
        var anime = await _helper.SearchAsync(parameters);
        return Ok(anime);
    }

    [HttpGet]
    [Route("year/{year:int:min(1900)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByReleaseYearAsync([FromRoute] int year)
    {
        var anime = await _helper.GetByReleaseYearAsync(year);
        if (!anime.Any()) return NotFound();
        
        return Ok(anime);
    }

    [HttpGet]
    [Route("episodes/{episodes:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByEpisodes([FromRoute] int episodes)
    {
        var anime = await _helper.GetByEpisodesAsync(episodes);
        if(!anime.Any()) return NotFound();
        
        return Ok(anime);
    }

    [HttpGet]
    [Route("title/{title}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByTitleAsync([FromRoute] string title)
    {
        var anime = await _helper.GetByNameAsync(title);
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