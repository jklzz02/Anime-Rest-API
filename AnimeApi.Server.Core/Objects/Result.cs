using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects;

public sealed class Result<T>
{
    [JsonProperty("data")]
    public T Data { get; }

    [JsonIgnore]
    public IEnumerable<Error> Errors { get; }

    [JsonIgnore]
    public bool IsSuccess
        => !Errors.Any();

    [JsonIgnore]
    public bool IsFailure
        => !IsSuccess;

    [JsonIgnore]
    public List<Error> InternalErrors
        => Errors.Where(e => e.IsInternal).ToList();

    [JsonProperty("errors")]
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
        Errors = errors;
    }

    public static Result<T> Success(T data)
    {
        return new(data);
    }

    public static Result<T> Failure(Error error)
    {
        return new ([ error ]);
    }

    public static Result<T> Failure(IEnumerable<Error> errors)
    {
        return new (errors);
    }

    public static Result<T> Failure(T data, IEnumerable<Error> errors)
    {
        return new (data, errors);
    }

    public static Result<T> InternalFailure(string error, string details)
        => Failure(Error.Internal(error, details));

    public static Result<T> ValidationFailure(string error, string details)
        => Failure(Error.Validation(error, details));
}