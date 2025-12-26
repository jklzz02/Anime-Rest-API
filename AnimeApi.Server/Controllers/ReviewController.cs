using System.Security.Claims;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewHelper _reviewHelper;
    private readonly IUserService _userService;

    public ReviewController(IReviewHelper reviewHelper, IUserService userService)
    {
        _reviewHelper = reviewHelper;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(int page = 1, int size = Constants.Pagination.MaxPageSize)
    {
        if (size > Constants.Pagination.MaxPageSize)
            return BadRequest($"Page size cannot be greater than '{Constants.Pagination.MaxPageSize}'");
        
        if (page <= 0)
            return  BadRequest($"Page must be greater than zero");
        
        var result = await _reviewHelper.GetAllAsync(page, size);

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
        var review = await _reviewHelper.GetByIdAsync(id);

        if (review is null)
        {
            return NotFound();
        }

        return Ok(review);
    }

    [HttpGet]
    [Route("anime/{animeId:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAnimeIdAsync([FromRoute] int animeId)
    {
        var reviews = await _reviewHelper.GetByAnimeIdAsync(animeId);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("anime/title/{title:minlength(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAnimeTitleAsync([FromRoute] string title) 
    {
        var reviews = await _reviewHelper.GetByTitleAsync(title);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("user/{userId:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] int id) 
    {
        var reviews = await _reviewHelper.GetByUserIdAsync(id);

        if (!reviews.Any()) 
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("user/email/{email:minlength(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserEmailAsync([FromRoute] string email)
    {
        var reviews = await _reviewHelper.GetByUserEmailAsync(email);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("date")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDateAsync([FromQuery] DateTime date)
    {
        var reviews = await _reviewHelper.GetByDateAsync(date);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("recent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMostRecentAsync([FromQuery] TimeSpan timeSpan)
    {
        var reviews = await _reviewHelper.GetMostRecentByTimeSpanAsync(timeSpan);

        if (!reviews.Any())
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet]
    [Route("score")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByScoreAsync([FromQuery] int minScore, [FromQuery] int maxScore)
    {
        if (minScore >= maxScore)
        {
            return BadRequest("The min score cannot be greater than or equal to the max score.");
        }

        if (minScore <= 0)
        {
            return BadRequest("The min score cannot be less than or equal to zero.");
        }

        if (maxScore > 10)
        {
            return BadRequest("The max score cannot be greater than ten.");
        }
        
        var reviews = await _reviewHelper.GetByScoreAsync(minScore, maxScore);

        if (!reviews.Any()) 
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

        var user = await _userService.GetByEmailAsync(email);

        if (user is null)
        {
            return Unauthorized();
        }

        if (user.Id != review.UserId) 
        {
            return Forbid();
        }

        var result = await _reviewHelper.CreateAsync(review);
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(result.Data);
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

        var user = await _userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }

        if (user.Id != review.UserId) 
        {
            return Forbid();
        }

        var result = await _reviewHelper.UpdateAsync(review);
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
    public async Task<IActionResult> UpdatePartialAsync(int id, [FromBody] JsonPatchDocument<ReviewDto> patchDocument)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }
        
        var user = await _userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }
        
        var review = await _reviewHelper.GetByIdAsync(id);
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
        
        var result = await _reviewHelper.UpdateAsync(review);
        if (result.IsFailure)
        {
            return BadRequest(result.ValidationErrors.ToKeyValuePairs());
        }
        
        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var email = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = await _userService.GetByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized();
        }

        var review = await _reviewHelper.GetByIdAsync(id);
        if (review is null)
        {
            return NotFound();
        }

        if (review.UserId != user.Id)
        {
            return Forbid();
        }

        var result = await _reviewHelper.DeleteAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}