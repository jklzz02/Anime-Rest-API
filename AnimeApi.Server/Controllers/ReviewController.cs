using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController(IReviewHelper reviewHelper, IUserService userService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] Pagination pagination)
    {
        var result = await reviewHelper.GetAllAsync(pagination.Page, pagination.Size);

        if (!result.HasItems)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpGet("detailed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDetailedAsync(
        [FromQuery] Pagination pagination)
    {
        var result = await
            reviewHelper
                .GetAllDetailedAsync(pagination.Page, pagination.Size);

        if (!result.HasItems)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id)
    {
        var review = await reviewHelper.GetByIdAsync(id);

        if (review is null)
        {
            return NotFound();
        }

        return Ok(review);
    }

    [HttpGet("detailed/{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailedByIdAsync([FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int id)
    {
        var review = await
            reviewHelper
                .GetDetailedByIdAsync(id);

        if (review is null)
        {
            return NotFound();
        }
        
        return Ok(review);
    }

    [HttpGet("anime/{animeId:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAnimeIdAsync([FromRoute, Range(1, int.MaxValue), DefaultValue(1)] int animeId)
    {
        var reviews = await reviewHelper.GetByAnimeIdAsync(animeId);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("anime/title/{title:minlength(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAnimeTitleAsync(
        [FromRoute, MaxLength(Constants.MaxTextQueryLength), MinLength(1)] string title) 
    {
        var reviews = await reviewHelper.GetByTitleAsync(title);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("q")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByQueryAsync(
        [FromRoute, MaxLength(Constants.MaxTextQueryLength), MinLength(1)] string query)
    {
        var result = await reviewHelper.GetByTextSearchAsync(query);

        if (result.ValidationErrors.Any())
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        if (!result.Data.Any())
        {
            return NotFound();
        }
        
        return Ok(result.Data);
    }

    [HttpGet("user/{userId:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute, Range(1, int.MaxValue) ,DefaultValue(1)] int userId) 
    {
        var reviews = await
            reviewHelper
                .GetByUserIdAsync(userId);

        if (!reviews.Any()) 
        {
            return NotFound();
        }

        return Ok(reviews);
    }
    
    [HttpGet("user/{userId:int:min(1)}/detailed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailedByUserIdAsync([FromRoute, Range(1, int.MaxValue) ,DefaultValue(1)] int userId) 
    {
        var reviews = await
            reviewHelper
                .GetDetailedByUserIdAsync(userId);

        if (!reviews.Any()) 
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("user/email/{email:minlength(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserEmailAsync([FromRoute] string email)
    {
        var reviews = await reviewHelper.GetByUserEmailAsync(email);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("date")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDateAsync([FromQuery] DateTime date)
    {
        var reviews = await reviewHelper.GetByDateAsync(date);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("recent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMostRecentAsync([FromQuery] TimeSpan timeSpan)
    {
        var reviews = await reviewHelper.GetMostRecentByTimeSpanAsync(timeSpan);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("score")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByScoreAsync(
        [FromQuery, Range(1, 10)] int minScore,
        [FromQuery, Range(1, 10)] int maxScore)
    {
        var result = await reviewHelper.GetByScoreAsync(minScore, maxScore);

        if (result.ValidationErrors.Any())
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        if (!result.Data.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync([FromBody] ReviewDto review)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = await userService.GetByEmailAsync(email);

        if (user is null)
        {
            return Unauthorized();
        }

        if (user.Id != review.UserId) 
        {
            return Forbid();
        }

        var result = await reviewHelper.CreateAsync(review);
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return CreatedAtAction(
            "GetById",
            new {id = result.Data.Id},
            result.Data);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync([FromBody] ReviewDto review)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = await userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }

        if (user.Id != review.UserId) 
        {
            return Forbid();
        }

        var result = await reviewHelper.UpdateAsync(review);
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPatch]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdatePartialAsync(
        [FromRoute, Range(1, int.MaxValue)] int id, 
        [FromBody] JsonPatchDocument<ReviewDto> patchDocument)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }
        
        var user = await userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }
        
        var review = await reviewHelper.GetByIdAsync(id);
        if (review is null)
        {
            return NotFound();
        }

        if (review.UserId != user.Id)
        {
            return Forbid();
        }
        
        patchDocument.ApplyTo(review, ModelState);
        if (!TryValidateModel(review))
        {
            return BadRequest(ModelState);
        }
        
        var result = await reviewHelper.UpdateAsync(review);
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = await userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }

        var review = await reviewHelper.GetByIdAsync(id);
        if (review is null)
        {
            return NotFound();
        }

        if (review.UserId != user.Id)
        {
            return Forbid();
        }

        var result = await reviewHelper.DeleteAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}