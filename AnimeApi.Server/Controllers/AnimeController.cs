using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimeController(
    IAnimeHelper helper,
    ICachingService cachingService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination, 
        bool includeAdultContent = false)
    {
        var result = await
            cachingService
                .GetOrCreateAsync(
                    () => helper.GetAllAsync(pagination.Page, pagination.Size, includeAdultContent),
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
    public async Task<IActionResult> GetById([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var anime = await cachingService
            .GetOrCreateAsync(
                () => helper.GetByIdAsync(id));
        
        if (anime is null)
        {
            return NotFound();
        }

        return Ok(anime);
    }

    [HttpPost("by-ids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIds([FromBody] TargetAnimeParams targetParams)
    {
        var anime = await cachingService
            .GetOrCreateAsync(
                () => helper.GetByIdAsync(
                    targetParams.TargetAnimeIds,
                    targetParams.OrderBy,
                    targetParams.SortOrder));

        if (anime is null)
        {
            return NotFound();
        }

        return Ok(anime);
    }

    [HttpGet("top/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTop(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var result = await helper.GetAsync(count);
        return Ok(result);
    }

    [HttpGet("by-query")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByQuery([FromQuery] AnimeListQuery request)
    {
        var list = string.IsNullOrEmpty(request.Query) || request.Query.Length < 3
            ? await helper.GetAsync(request.Count)
            : await helper.GetByQueryAsync(request.Query, request.Count);
        
        return Ok(list);
    }

    [HttpGet("recent/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecent(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var result = await helper.GetMostRecentAsync(count);
        return Ok(result);
    }
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Search(
        [FromQuery] AnimeSearchParameters parameters,
        [FromQuery] Pagination pagination)
    {
        var result = await 
            cachingService
                .GetOrCreateAsync(
                    new { parameters, pagination.Page, pagination.Size},
                    () =>helper.SearchAsync(parameters, pagination.Page, pagination.Size),
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

    [HttpGet("summaries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSummaries(
        [FromQuery] Pagination pagination, 
        bool includeAdultContent = false)
    {
        var result = await
            cachingService
                .GetOrCreateAsync(
                    () => helper.GetAllAsync<AnimeSummary>(pagination.Page, pagination.Size, includeAdultContent),
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

    [HttpGet("summaries/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummaryById([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var summary = await helper.GetByIdAsync<AnimeSummary>(id);
        
        if (summary is null)
        {
            return NotFound();
        }
        
        return Ok(summary);
    }
    
    [HttpPost("summaries/by-ids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummariesByIds([FromBody] TargetAnimeParams targetParams)
    {
        var summaries = await cachingService
            .GetOrCreateAsync(
                () => helper.GetByIdAsync<AnimeSummary>(
                    targetParams.TargetAnimeIds,
                    targetParams.OrderBy,
                    targetParams.SortOrder));

        if (summaries is null)
        {
            return NotFound();
        }
        
        return Ok(summaries);
    }

    [HttpGet("summaries/top/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopSummaries(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var summaries = await helper.GetAsync<AnimeSummary>(count);
        return Ok(summaries);
    }

    [HttpGet("summaries/by-query")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummariesByQuery([FromQuery] AnimeListQuery request)
    {
        var list = string.IsNullOrEmpty(request.Query) || request.Query.Length < 3
            ? await helper.GetAsync<AnimeSummary>(request.Count)
            : await helper.GetByQueryAsync<AnimeSummary>(request.Query, request.Count);
        
        return Ok(list);
    }

    [HttpGet("summaries/recent/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentSummaries(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var result = await helper.GetMostRecentAsync<AnimeSummary>(count);
        return Ok(result);
    }

    [HttpGet("summaries/search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchSummaries(
        [FromQuery] AnimeSearchParameters parameters,
        [FromQuery] Pagination pagination)
    {
        var result = await 
            cachingService
                .GetOrCreateAsync(
                    new { parameters, pagination.Page, pagination.Size},
                    () =>helper.SearchAsync<AnimeSummary>(parameters, pagination.Page, pagination.Size),
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

    [HttpGet("list-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllListItems(
        [FromQuery] Pagination pagination, 
        bool includeAdultContent = false)
    {
        var result = await
            cachingService
                .GetOrCreateAsync(
                    () => helper.GetAllAsync<AnimeListItem>(pagination.Page, pagination.Size, includeAdultContent),
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

    [HttpGet("list-items/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListItemById([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var listItem = await helper.GetByIdAsync<AnimeListItem>(id);
        
        if (listItem is null)
        {
            return NotFound();
        }
        
        return Ok(listItem);
    }

    [HttpPost("list-items/by-ids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListItemsByIds([FromBody] TargetAnimeParams targetParams)
    {
        var listItems = await cachingService
            .GetOrCreateAsync(
                () => helper.GetByIdAsync<AnimeListItem>(
                    targetParams.TargetAnimeIds,
                    targetParams.OrderBy,
                    targetParams.SortOrder));

        if (listItems is null)
        {
            return NotFound();
        }
        
        return Ok(listItems);
    }

    [HttpGet("list-items/top/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopListItems(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var listItems = await helper.GetAsync<AnimeListItem>(count);
        return Ok(listItems);
    }

    [HttpGet("list-items/by-query")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListItemsByQuery([FromQuery] AnimeListQuery request)
    {
        var list = string.IsNullOrEmpty(request.Query) || request.Query.Length < 3
            ? await helper.GetAsync<AnimeListItem>(request.Count)
            : await helper.GetByQueryAsync<AnimeListItem>(request.Query, request.Count);
        
        return Ok(list);
    }

    [HttpGet("list-items/recent/{count:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentListItems(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(Constants.DefaultRetrieveCount)] int count)
    {
        var result = await helper.GetMostRecentAsync<AnimeListItem>(count);
        return Ok(result);
    }

    [HttpGet("list-items/search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchListItems(
        [FromQuery] AnimeSearchParameters parameters,
        [FromQuery] Pagination pagination)
    {
        var result = await 
            cachingService
                .GetOrCreateAsync(
                    new { parameters, pagination.Page, pagination.Size},
                    () =>helper.SearchAsync<AnimeListItem>(parameters, pagination.Page, pagination.Size),
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> Create([FromBody] AnimeDto anime)
    {
        var result = await helper.CreateAsync(anime);
        
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data.Id },
            result.Data);
    }
    
    [HttpPatch("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> UpdatePartial(
        [FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id,
        [FromBody] JsonPatchDocument<AnimeDto> patchDocument)
    {
        var anime = await helper.GetByIdAsync(id);

        if (anime is null)
        {
            return NotFound();
        }
        
        patchDocument.ApplyTo(anime, ModelState);

        if (!TryValidateModel(anime))
        {
            return BadRequest(ModelState);
        }
        
        var result = await helper.UpdateAsync(anime);
        
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
    public async Task<IActionResult> Update([FromBody] AnimeDto anime)
    {
        var result = await helper.UpdateAsync(anime);
        
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
    public async Task<IActionResult> Delete([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var result = await helper.DeleteAsync(id);

        if (!result) 
        {
            return NotFound();
        }
        
        return NoContent();
    }
}