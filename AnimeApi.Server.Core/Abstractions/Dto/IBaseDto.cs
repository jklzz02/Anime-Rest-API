namespace AnimeApi.Server.Core.Abstractions.Dto;

/// <summary>
/// Represents a base interface for Data Transfer Objects (DTOs).
/// Provides common properties that are shared across all implementing DTO classes.
/// </summary>
public interface IBaseDto
{ 
    int? Id { get; init; }
    string? Name { get; init; }
}