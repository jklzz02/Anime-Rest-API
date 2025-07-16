using AnimeApi.Server.Business;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TypeController : ControllerBase
{
    private readonly ITypeHelper _helper;
    
    public TypeController(ITypeHelper helper)
    {
        _helper = helper;
    }
    
    [HttpGet]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var types = await _helper.GetAllAsync();
        return Ok(types);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var type = await _helper.GetByIdAsync(id);
        if (type is null) return NotFound();
        
        return Ok(type);
    }
    
    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var type = await _helper.GetByNameAsync(name);
        if (!type.Any()) return NotFound();
        
        return Ok(type);
    }

    [HttpPost]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] TypeDto type)
    {
        var result = await _helper.CreateAsync(type);
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
    public async Task<IActionResult> UpdateFullAsync([FromBody] TypeDto type)
    {
        var result = await _helper.UpdateAsync(type);
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
    public async Task<IActionResult> UpdatePartialAsync([FromBody] TypeDto type)
    {
        var result = await _helper.UpdateAsync(type);
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