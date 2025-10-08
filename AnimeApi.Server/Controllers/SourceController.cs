using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SourceController : Controller
{
    private readonly ISourceHelper _helper;
    
    public SourceController(ISourceHelper helper)
    {
        _helper = helper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var sources = await _helper.GetAllAsync();
        return Ok(sources);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var source = await _helper.GetByIdAsync(id);
        if (source is null) return NotFound();
        
        return Ok(source);
    }
    
    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var source = await _helper.GetByNameAsync(name);
        
        if (!source.Any()) 
        {
            return NotFound();
        }
        
        return Ok(source);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] SourceDto source)
    {
        var result = await _helper.CreateAsync(source);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.TokeyValuePairs());
        }
        
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] SourceDto source)
    {
        var result = await _helper.UpdateAsync(source);

        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.TokeyValuePairs());
        }
        
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
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