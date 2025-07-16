using AnimeApi.Server.Business;
using AnimeApi.Server.Business.Services.Interfaces;
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
    [ProducesResponseType(Constant.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var types = await _helper.GetAllAsync();
        return Ok(types);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var type = await _helper.GetByIdAsync(id);
        if (type is null) return NotFound();
        
        return Ok(type);
    }
    
    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var type = await _helper.GetByNameAsync(name);
        if (!type.Any()) return NotFound();
        
        return Ok(type);
    }

    [HttpPost]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.BadRequest)]
    [ProducesResponseType(Constant.StatusCode.Forbidden)]
    [Authorize(Policy = Constant.UserAccess.Admin)]
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
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.BadRequest)]
    [ProducesResponseType(Constant.StatusCode.Forbidden)]
    [Authorize(Policy = Constant.UserAccess.Admin)]
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
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.BadRequest)]
    [ProducesResponseType(Constant.StatusCode.Forbidden)]
    [Authorize(Policy = Constant.UserAccess.Admin)]
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
    [ProducesResponseType(Constant.StatusCode.NoContent)]
    [ProducesResponseType(Constant.StatusCode.Forbidden)] 
    [ProducesResponseType(Constant.StatusCode.NotFound)]
    [Authorize(Policy = Constant.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _helper.DeleteAsync(id);
        if(!result) return NotFound();

        return NoContent();
    }
}