using System.Text;

namespace AnimeApi.Server.Core.Objects;
public record Error
{
    public ErrorType Type { get; }
    
    public string Message { get; }
    
    public string Details { get; }
    
    public Error(ErrorType type, string message, string details)
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
}

public enum ErrorType
{
    Validation,
    Internal,
}
