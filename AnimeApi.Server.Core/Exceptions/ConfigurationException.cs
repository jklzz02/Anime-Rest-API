using Microsoft.Extensions.Configuration;

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

    public static void ThrowIfMissing(IConfiguration configuration, string configurationKey)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(configurationKey);

        if (!configuration.GetSection(configurationKey).Exists())
        {
            throw new ConfigurationException(configurationKey, $"Required configuration key '{configurationKey}' is missing");
        }
    }
    
    public static void ThrowIfEmpty(IConfiguration configuration, string configurationKey)
    {
        ThrowIfMissing(configuration, configurationKey);
        
        if (string.IsNullOrWhiteSpace(configuration.GetSection(configurationKey).Value))
        {
            throw  new ConfigurationException(configurationKey, $"Required configuration value '{configurationKey}' is empty");
        }
    }
}