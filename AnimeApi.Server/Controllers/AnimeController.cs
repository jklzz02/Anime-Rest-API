using System.Net;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimeController : ControllerBase
{
    private readonly IAnimeHelper _helper;
    private readonly ICachingService _cache;
    private readonly string _recommenderDomain;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public AnimeController(
        IAnimeHelper helper,
        ICachingService cachingService,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _helper = helper;
        _cache = cachingService;
        _recommenderDomain = configuration
            .GetValue<string>("Authorization:RecommenderDomain")!;
        
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllsync([FromQuery] int page = 1, int size = Constants.Pagination.MaxPageSize, bool includeAdultContent = false)
    {
         var result = await _cache
             .GetOrCreateAsync(
                 () => includeAdultContent ? 
                     _helper.GetAllAsync(page, size) :
                     _helper.GetAllNonAdultAsync(page, size), 
                 Constants.Cache.MinCachedItemSize);
         
        if (!result.Success)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        if (!result.HasItems) 
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                () => _helper.GetByIdAsync(id));
        
        if (anime is null)
        {
            return NotFound();
        }

        return Ok(anime);
    }
    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByParametersAsync(
        [FromQuery] AnimeSearchParameters parameters,
        int page = 1,
        int size = Constants.Pagination.MaxPageSize)
    {
        var result = await 
            _cache
                .GetOrCreateAsync(
                    new { parameters, page, size },
                    () =>_helper.SearchAsync(parameters, page, size),
                    Constants.Cache.DefaultCachedItemSize,
                    TimeSpan.FromMinutes(2));
        
        if (!result.Success)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        if (!result.HasItems) 
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummariesAsync([FromQuery] int count)
    {
        var result = await _helper.GetSummariesAsync(count);
        return Ok(result);
    }

    [HttpGet]
    [Route("related")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRelatedAsync([FromQuery] int id, int count = 10)
    {
        var http = _httpClientFactory.CreateClient();
        var response = await
            http.GetAsync($"{_recommenderDomain}/v1/recommend?anime_id={id}&limit={count}");

        if (response is null || response.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Request failed");
        }
        
        var ids = await response.Content
            .ReadFromJsonAsync<List<int>>() ?? [];

        var result = await _helper.GetByIdsAsync(ids);
        
        return Ok(result);
    }

    [HttpGet]
    [Route("recent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentAsync([FromQuery]int count)
    {
        var result = await _helper.GetMostRecentAsync(count);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.CreateAsync(anime);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        };

        return Ok(result.Data);
    }
    
    [HttpPatch]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartialAsync(int id, [FromBody] JsonPatchDocument<AnimeDto> patchDocument)
    {
        var anime = await _helper.GetByIdAsync(id);

        if (anime is null)
        {
            return NotFound();
        }
        
        patchDocument.ApplyTo(anime, ModelState);

        if (!TryValidateModel(anime))
        {
            return BadRequest(ModelState);
        }
        
        var result = await _helper.UpdateAsync(anime);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdateFullAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.UpdateAsync(anime);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]   
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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