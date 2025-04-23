using AnimeApi.Server.Business.Service.Helpers;
using AnimeApi.Server.Business.Service.Interfaces;
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
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
    {
        var anime = await _helper.GetByIdAsync(id);
        if (anime is null) return NotFound();
        
        return Ok(anime);
    }    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromQuery] AnimeSearchParameters parameters)
    {
        var anime = await _helper.SearchAsync(parameters);
        return Ok(anime);
    }
}