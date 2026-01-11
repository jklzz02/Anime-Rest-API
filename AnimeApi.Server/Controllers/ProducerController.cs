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
public class ProducerController : ControllerBase
{
    private readonly IProducerHelper _helper;
    private readonly ICachingService _cache;
    
    public ProducerController(IProducerHelper helper, ICachingService cache)
    {
        _helper = helper;
        _cache = cache;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var producers = await
            _cache.GetOrCreateAsync(
                () => _helper.GetAllAsync(),
                Constants.Cache.MinCachedItemSize);
        
        return Ok(producers);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id)
    {
        var producer = await _helper.GetByIdAsync(id);
        
        if (producer is null)
        {
            return NotFound();
        } 
        
        return Ok(producer);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var producer = await _helper.GetByNameAsync(name);
        if (!producer.Any()) return NotFound();
        
        return Ok(producer);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] ProducerDto producer)
    {
        var result = await _helper.CreateAsync(producer);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        return CreatedAtAction(
            "GetById",
            new {id = result.Data.Id},
            result.Data);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] ProducerDto producer)
    {
        if (string.IsNullOrEmpty(producer.Name))
        {
            return BadRequest();
        }
        
        var result = await _helper.UpdateAsync(producer);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        return Ok(result.Data);
    }
    
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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