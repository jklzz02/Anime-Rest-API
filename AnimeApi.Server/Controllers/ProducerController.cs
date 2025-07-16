using AnimeApi.Server.Business;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(Constants.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var producers = await _helper.GetAllAsync();
        return Ok(producers);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var producer = await _helper.GetByIdAsync(id);
        if (producer is null) return NotFound();
        
        return Ok(producer);
    }

    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var producer = await _helper.GetByNameAsync(name);
        if (!producer.Any()) return NotFound();
        
        return Ok(producer);
    }

    [HttpPost]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] ProducerDto producer)
    {
        var result = await _helper.CreateAsync(producer);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Unauthorized)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] ProducerDto producer)
    {
        if (string.IsNullOrEmpty(producer.Name))
        {
            return BadRequest();
        }
        var result = await _helper.UpdateAsync(producer);
        if (result is null)
        {
            return BadRequest(_helper.ErrorMessages);
        }
        
        return Ok(result);
    }
    
    [HttpPatch]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Unauthorized)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartialAsync([FromBody] ProducerDto producer)
    {
        if (string.IsNullOrEmpty(producer.Name))
        {
            return BadRequest();
        }
        var result = await _helper.UpdateAsync(producer);
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
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);
        if(!result) return NotFound();
        
        return NoContent();
    }
}