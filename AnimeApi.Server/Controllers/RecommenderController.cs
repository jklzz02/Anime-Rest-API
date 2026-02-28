using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects.Partials;
using AnimeApi.Server.Recommender.Grpc;
using AnimeApi.Server.RequestModels.Recommender;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecommenderController(
    AnimeRecommender.AnimeRecommenderClient recommenderClient,
    IAnimeHelper helper)
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

            if (response is null)
            {
                return NotFound();
            }

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