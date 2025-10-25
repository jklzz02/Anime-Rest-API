using System.Text;

namespace AnimeApi.Server.Core.Objects;

/// <summary>
/// Represents an error with a specific type, message, and details.
/// </summary>
public record Error
{
    public ErrorType Type { get; }
    
    public string Message { get; }
    
    public string Details { get; }
    
    private Error(ErrorType type, string message, string details)
    {
        Type = type;
        Message = message;
        Details = details;
    }

    public bool IsValidation
        => Type == ErrorType.Validation;
    public bool IsInternal
        => Type == ErrorType.Internal;
    
    public override string ToString()
    {
        StringBuilder sb = new();

        if (IsValidation)
        {
            sb.Append("Validation ");
        }
        
        if (IsInternal)
        {
            sb.Append("Internal ");
        }

        sb.Append($"Error: {Message}\n Details: {Details}");
        return sb.ToString();
    }

    public override int GetHashCode()
        => HashCode.Combine(Type, Message, Details);

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> class with the type set to <see cref="ErrorType.Validation"/>.
    /// </summary>
    /// <param name="message">The message describing the validation error.</param>
    /// <param name="details">Additional details about the validation error.</param>
    /// <returns>A <see cref="Error"/> instance representing a validation error.</returns>
    public static Error Validation(string message, string details)
        => new(ErrorType.Validation, message, details);

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> class with the type set to <see cref="ErrorType.Internal"/>.
    /// </summary>
    /// <param name="message">The message describing the internal error.</param>
    /// <param name="details">Additional details about the internal error.</param>
    /// <returns>A <see cref="Error"/> instance representing an internal error.</returns>
    public static Error Internal(string message, string details)
        => new(ErrorType.Internal, message, details);
}

/// <summary>
/// Specifies the type of error that occurred.
/// </summary>
public enum ErrorType
{
    Validation,
    Internal,
}
