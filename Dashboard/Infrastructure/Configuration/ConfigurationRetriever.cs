using Domain.Configuration;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Infrastructure.Configuration
{
    public class ConfigurationRetriever : IConfigurationRetriever
    {
        private IConfiguration? _configuration = null;

        public ConfigurationRetriever(IConfiguration? configuration)
        {
            if (configuration != null)
            {
                _configuration = configuration;
            }
        }

        public string? Get(string key)
        {
            string? value;

            if (_configuration != null)
            {
                value = _configuration[key];
                if (value != null) return value;
            }

            value = Environment.GetEnvironmentVariable(key);
            if (value != null) return value;

            throw new ConfigurationErrorsException($"Config value for '{key}' not found, or is empty. Ensure it exists in appsettings.Secrets.json or other config file.");
        }
    }
}