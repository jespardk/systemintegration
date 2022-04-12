using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace DomainServices.Configuration
{
    public class ConfigurationService
    {
        private IConfiguration? _configuration = null;

        public ConfigurationService(IConfiguration? configuration)
        {
            if (configuration != null)
            {
                _configuration = configuration;
            }
        }

        public string? GetConfigValue(string key)
        {
            string? value;

            if (_configuration != null)
            {
                value = _configuration[key];
                if (value != null) return value;
            }

            value = Environment.GetEnvironmentVariable(key);
            if (value != null) return value;

            throw new ConfigurationErrorsException($"Config value for '{key}' not found, or is empty.");
        }
    }
}