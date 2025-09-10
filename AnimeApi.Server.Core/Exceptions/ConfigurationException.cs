namespace AnimeApi.Server.Core.Exceptions;

/// <summary>
/// Error raised when a required configuration value is missing or empty.
/// </summary>
public class ConfigurationException : Exception
{
    public string ConfigurationKey { get; }

    public ConfigurationException(string configurationKey) 
        : base($"Required configuration value '{configurationKey}' is missing or empty")
    {
        ConfigurationKey = configurationKey;
    }

    public ConfigurationException(string configurationKey, string message) 
        : base(message)
    {
        ConfigurationKey = configurationKey;
    }

    public ConfigurationException(string configurationKey, string message, Exception innerException) 
        : base(message, innerException)
    {
        ConfigurationKey = configurationKey;
    }
}