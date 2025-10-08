using System.Text;

namespace AnimeApi.Server.Core.Objects;
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

    public static Error Validation(string message, string details)
        => new(ErrorType.Validation, message, details);
    
    public static Error Internal(string message, string details)
        => new(ErrorType.Internal, message, details);
}

public enum ErrorType
{
    Validation,
    Internal,
}
