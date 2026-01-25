namespace AnimeApi.Server.Core.Abstractions.Dto;

/// <summary>
/// Represents a marker interface used to specify that a class or record can be projected
/// from a source type into a specified destination type.
/// </summary>
/// <typeparam name="TDestination">
/// The type from which the implementing class or record can project data.
/// </typeparam>
public interface IProjectableFrom<TDestination>
{
}