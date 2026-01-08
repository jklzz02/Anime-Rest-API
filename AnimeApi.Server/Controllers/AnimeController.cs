using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimeController : ControllerBase
{
    private readonly IAnimeHelper _helper;
    private readonly ICachingService _cache;
    
    public AnimeController(
        IAnimeHelper helper,
        ICachingService cachingService)
    {
        _helper = helper;
        _cache = cachingService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] Pagination pagination, 
        bool includeAdultContent = false)
    {
        var result = await
            _cache
                .GetOrCreateAsync(
                    () => _helper.GetAllAsync(pagination.Page, pagination.Size, includeAdultContent),
                    Constants.Cache.DefaultCachedItemSize);

        if (result is null)
        {
            return NotFound();
        }

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
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute, Range(1, int.MaxValue)] int id)
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

    [HttpPost]
    [Route("target")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdsAsync([FromBody] TargetAnimeParams targetParams)
    {
        var anime = await _cache
            .GetOrCreateAsync(
                () => _helper.GetByIdAsync(
                    targetParams.TargetAnimeIds,
                    targetParams.OrderBy,
                    targetParams.SortOrder));

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
        [FromQuery] Pagination pagination)
    {
        var result = await 
            _cache
                .GetOrCreateAsync(
                    new { parameters, pagination.Page, pagination.Size },
                    () =>_helper.SearchAsync(parameters, pagination.Page, pagination.Size),
                    Constants.Cache.DefaultCachedItemSize,
                    TimeSpan.FromMinutes(2));

        if (result is null)
        {
            return NotFound();
        }
        
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

    [HttpGet("summary/{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummaryAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var summary = await _helper.GetSummaryByIdAsync(id);
        
        if (summary is null)
        {
            return NotFound();
        }
        
        return Ok(summary);
    }

    [HttpGet("summaries/count/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummariesAsync(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var summaries = await _helper.GetSummariesAsync(count);
        return Ok(summaries);
    }
    
    [HttpPost("summaries/target")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummariesByIdsAsync([FromBody] TargetAnimeParams targetParams)
    {
        var summaries = await _cache
            .GetOrCreateAsync(
                () => _helper.GetSummariesByIdAsync(
                    targetParams.TargetAnimeIds,
                    targetParams.OrderBy,
                    targetParams.SortOrder));

        if (summaries is null)
        {
            return NotFound();
        }
        
        return Ok(summaries);
    }
    
    [HttpGet("recent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentAsync(
        [FromQuery, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var result = await _helper.GetMostRecentAsync(count);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody] AnimeDto anime)
    {
        var result = await _helper.CreateAsync(anime);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return CreatedAtAction(
            "GetById",
            new { id = result.Data.Id },
            result.Data);
    }
    
    [HttpPatch("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartialAsync(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id,
        [FromBody] JsonPatchDocument<AnimeDto> patchDocument)
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
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]   
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var result = await _helper.DeleteAsync(id);

        if (!result) 
        {
            return NotFound();
        }
        
        return NoContent();
    }
}