
namespace AnimeApi.Server.Core.Objects;

/// <summary>
/// Represents the result of an operation, combining success or failure state,
/// a data object on success, and a collection of errors on failure.
/// </summary>
/// <typeparam name="T">The type of the data object contained in this result on success.</typeparam>
public sealed class Result<T>
{
    /// <summary>
    /// Represents the data contained in the result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of the data object.</typeparam>
    /// <remarks>
    /// This property holds the core data resulting from a successful operation.
    /// It is null when the operation has not succeeded.
    /// </remarks>
    public T Data { get; }

    /// <summary>
    /// Represents a collection of errors that occurred during the operation.
    /// </summary>
    /// <remarks>
    /// This property holds detailed information about the errors encountered.
    /// If the operation fails, these errors provide context about the failure,
    /// such as validation or internal processing issues.
    /// The collection is empty in case of a successful operation.
    /// </remarks>
    public IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// Indicates whether the operation represented by this result was successful.
    /// </summary>
    /// <value>
    /// true if the operation completed successfully; otherwise, false.
    /// </value>
    public bool IsSuccess
        => !Errors.Any();

    /// <summary>
    /// Indicates whether an operation has failed.
    /// </summary>
    /// <value>
    /// true if the operation failed; otherwise, false.
    /// </value>
    public bool IsFailure
        => !IsSuccess;

    /// <summary>
    /// Gets a list of internal errors from the collection of all errors.
    /// </summary>
    public List<Error> InternalErrors
        => Errors.Where(e => e.IsInternal).ToList();

    /// <summary>
    /// Retrieves a collection of errors that are categorized as validation errors.
    /// </summary>
    public List<Error> ValidationErrors
        => Errors.Where(e => e.IsValidation).ToList();

    private Result(T data)
        : this(data, [])
    {
    }

    private Result(IEnumerable<Error> errors)
        : this(default!, errors)
    {
    }
    
    private Result(T data, IEnumerable<Error> errors)
    {
        Data = data;
        Errors = errors.ToList();
    }

    /// Creates a success result associated with the provided data.
    /// <param name="data">The data representing a successful result.</param>
    /// <returns>A Result object containing the specified data as a successful outcome.</returns>
    public static Result<T> Success(T data)
    {
        return new(data);
    }

    /// Creates a failure result associated with the provided error.
    /// <param name="error">The error that describes the reason for the failure.</param>
    /// <returns>A Result object indicating failure and containing the specified error.</returns>
    public static Result<T> Failure(Error error)
    {
        return new ([ error ]);
    }

    /// Creates a failure result using the specified collection of errors.
    /// <param name="errors">The collection of errors representing the failure reasons.</param>
    /// <returns>A Result object containing the provided errors as a failure outcome.</returns>
    public static Result<T> Failure(IEnumerable<Error> errors)
    {
        return new (errors);
    }

    /// Creates a failure result associated with the provided data and errors.
    /// <param name="data">The data related to the failure outcome.</param>
    /// <param name="errors">The collection of errors describing the failure.</param>
    /// <returns>A Result object indicating failure and containing the specified data and errors.</returns>
    public static Result<T> Failure(T data, IEnumerable<Error> errors)
    {
        return new (data, errors);
    }

    /// Creates an internal failure result associated with provided error message and details.
    /// <param name="error">The error message describing the internal failure.</param>
    /// <param name="details">The details providing additional context about the failure.</param>
    /// <returns>A failure Result object containing an internal error with the specified message and details.</returns>
    public static Result<T> InternalFailure(string error, string details)
        => Failure(Error.Internal(error, details));

    /// Creates a validation failure result associated with the provided error message and details.
    /// <param name="error">The error message describing the validation failure.</param>
    /// <param name="details">The details providing additional context about the validation failure.</param>
    /// <returns>A failure Result object containing a validation error with the specified message and details.</returns>
    public static Result<T> ValidationFailure(string error, string details)
        => Failure(Error.Validation(error, details));
}