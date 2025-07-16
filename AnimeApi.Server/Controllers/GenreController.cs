using AnimeApi.Server.Business;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreHelper _helper;
    public GenreController(IGenreHelper helper)
    {
        _helper = helper;
    }

    [HttpGet]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var genres = await _helper.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var genre = await _helper.GetByIdAsync(id);
        if (genre is null) return NotFound();
        
        return Ok(genre);
    }

    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var genre = await _helper.GetByNameAsync(name);
        if (!genre.Any()) return NotFound();
        
        return Ok(genre);
    }

    [HttpPost]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] GenreDto genre)
    {
        var result = await _helper.CreateAsync(genre);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] GenreDto genre)
    {
        if (string.IsNullOrEmpty(genre.Name))
        {
            return BadRequest();
        }
        var result = await _helper.UpdateAsync(genre);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        return Ok(result);
    }
    
    [HttpPatch]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartialAsync([FromBody] GenreDto genre)
    {
        if (string.IsNullOrEmpty(genre.Name))
        {
            return BadRequest();
        }
        
        var result = await _helper.UpdateAsync(genre);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constants.StatusCode.NoContent)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);
        if(!result) return NotFound();
        return NoContent();
    }
}

