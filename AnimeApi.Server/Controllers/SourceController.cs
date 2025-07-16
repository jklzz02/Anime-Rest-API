using AnimeApi.Server.Business;
using AnimeApi.Server.Core.Abstractions.Business.Services;
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
    [ProducesResponseType(Constant.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var sources = await _helper.GetAllAsync();
        return Ok(sources);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var source = await _helper.GetByIdAsync(id);
        if (source is null) return NotFound();
        
        return Ok(source);
    }
    
    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var source = await _helper.GetByNameAsync(name);
        if (!source.Any()) return NotFound();
        
        return Ok(source);
    }

    [HttpPost]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.BadRequest)]
    [ProducesResponseType(Constant.StatusCode.Forbidden)]
    [Authorize(Policy = Constant.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] SourceDto source)
    {
        var result = await _helper.CreateAsync(source);
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
    public async Task<IActionResult> UpdateFullAsync([FromBody] SourceDto source)
    {
        var result = await _helper.UpdateAsync(source);
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
    public async Task<IActionResult> UpdatePartialAsync([FromBody] SourceDto source)
    {
        var result = await _helper.UpdateAsync(source);
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