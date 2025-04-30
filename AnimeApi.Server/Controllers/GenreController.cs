using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
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
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var genres = await _helper.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var genre = await _helper.GetByIdAsync(id);
        if (genre is null) return NotFound();
        
        return Ok(genre);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] GenreDto genre)
    {
        var result = await _helper.CreateAsync(genre);
        if (!result) return BadRequest();

        return Ok(genre);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] GenreDto genre)
    {
        if(genre.Name == "") return BadRequest();
        var result = await _helper.UpdateAsync(genre);
        if(!result) return BadRequest();
        return Ok("Resource updated");
    }
    
    [HttpPatch]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdatePartialAsync([FromBody] GenreDto genre)
    {
        if(genre.Name == "") return BadRequest();
        var result = await _helper.UpdateAsync(genre);
        if(!result) return BadRequest();
        return Ok("Resource updated");
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);
        if(!result) return NotFound();
        return Ok("Resource deleted");
    }
}

