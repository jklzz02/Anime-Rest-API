using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.RequestModels;

public record UserDetailsRequest
{
    [FromQuery(Name = "email"), Required, MaxLength(255), EmailAddress(ErrorMessage = "Invalid email")]
    public required string Email { get; init; }
}