using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
    private readonly IProducerHelper _helper;
    
    public ProducerController(IProducerHelper helper)
    {
        _helper = helper;
    }
    
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var producers = await _helper.GetAllAsync();
        return Ok(producers);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var producer = await _helper.GetByIdAsync(id);
        if (producer is null) return NotFound();
        
        return Ok(producer);
    }

    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var producer = await _helper.GetByNameAsync(name);
        if (!producer.Any()) return NotFound();
        
        return Ok(producer);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] ProducerDto producer)
    {
        var result = await _helper.CreateAsync(producer);
        if (!result)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        return Ok(producer);
    }
}