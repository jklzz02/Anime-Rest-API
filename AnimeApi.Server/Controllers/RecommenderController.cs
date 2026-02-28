using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.Recommender.Grpc;
using AnimeApi.Server.RequestModels.Recommender;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecommenderController(
    AnimeRecommender.AnimeRecommenderClient recommenderClient,
    IAnimeHelper helper,
    IUserService userService)
    : ControllerBase
{
    [HttpGet("related")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetRelated(RelatedAnimeRequest request)
    {
        try
        {
            var message = new RelatedRequest
            {
                AnimeId = request.AnimeId,
                Limit = request.Count
            };

            var response = await recommenderClient.GetRelatedAsync(message);

            return request.EntityType switch
            {
                ResultEntityType.Full
                    => Ok(await helper.GetByIdAsync(response.AnimeIds)),
                    
                ResultEntityType.Summary
                    => Ok(await helper.GetByIdAsync<AnimeSummary>(response.AnimeIds)),
                        
                _ => Ok(await helper.GetByIdAsync<AnimeListItem>(response.AnimeIds)),
            };
            
        }
        catch (RpcException ex)
        {
            return RpcFailureResponse(ex);
        }
    }

    [Authorize]
    [HttpGet("compatibility/score")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetCompatibility(
        [FromQuery(Name = "target_anime_id"), Range(1, int.MaxValue)] int animeId)
    {
        var email = User.FindFirst(ClaimTypes.Email);
        
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }
        
        var favourites = await userService.GetFavouritesAsync(user.Id);
        
        try
        {
            var message = new CompatibilityRequest
            {
                TargetAnimeId = animeId,
                UserFavouriteIds = { favourites.Select(f => f.AnimeId) }
            };

            var response = await recommenderClient.GetCompatibilityAsync(message);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(new
            {
                target_anime_id = response.TargetAnimeId,
                compatibility_score = response.CompatibilityScore,
                scale = response.Scale
            });
        }
        catch (RpcException ex)
        {
            return RpcFailureResponse(ex);
        }
    }

    private IActionResult RpcFailureResponse(RpcException ex)
    {
        
        return ex.StatusCode switch
        {
            Grpc.Core.StatusCode.NotFound
                => NotFound(new { details = ex.Status.Detail }),
                
            Grpc.Core.StatusCode.Unavailable
                => StatusCode(
                    StatusCodes.Status503ServiceUnavailable, 
                    new { error = "Service Unavailable", details = ex.Status.Detail }),
                
            Grpc.Core.StatusCode.DeadlineExceeded
                => StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    new { error = "Gateway Timeout", details = ex.Status.Detail }),

            _ => throw new RpcException(ex.Status, ex.Trailers)
        };
    }
}