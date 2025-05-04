using AnimeApi.Server.Business.Services.Interfaces;
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
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var licensors = await _helper.GetAllAsync();
        return Ok(licensors);
    }
    
}