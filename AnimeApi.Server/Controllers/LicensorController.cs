using AnimeApi.Server.Business;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LicensorController : ControllerBase
{
    private readonly ILicensorHelper _helper;
    public LicensorController(ILicensorHelper helper)
    {
        _helper = helper;
    }
    
    [HttpGet]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    public async Task<IActionResult> GetAllAsync()
    {
        var licensors = await _helper.GetAllAsync();
        return Ok(licensors);
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var licensor = await _helper.GetByIdAsync(id);
        if(licensor is null) return NotFound();
        return Ok(licensor);
    }

    [HttpGet]
    [Route("name/{name}")]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var licensor = await _helper.GetByNameAsync(name);
        if(!licensor.Any()) return NotFound();
        
        return Ok(licensor);
    }

    [HttpPost]
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.BadRequest)]
    [ProducesResponseType(Constants.StatusCode.Unauthorized)]
    [ProducesResponseType(Constants.StatusCode.Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] LicensorDto licensor)
    {
        var result = await _helper.CreateAsync(licensor);
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
    public async Task<IActionResult> UpdateFullAsync([FromBody] LicensorDto licensor)
    {
        if (string.IsNullOrEmpty(licensor.Name))
        {
            return BadRequest();
        }
        
        var result = await _helper.UpdateAsync(licensor);
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
    public async Task<IActionResult> UpdatePartialAsync([FromBody] LicensorDto licensor)
    {
        if (string.IsNullOrEmpty(licensor.Name))
        {
            return BadRequest();
        }
        
        var result = await _helper.UpdateAsync(licensor);
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