using Microsoft.Extensions.Configuration;

namespace Case.Services
{
    public class ConfigurationService
    {
        private IConfiguration? _configuration = null;

        public ConfigurationService(IConfiguration configuration)
        {
            if (configuration != null)
            {
                _configuration = configuration;
            }
        }

        public string? GetConfigValue(string key)
        {
            if (_configuration != null)
            {
                var value = _configuration[key];
                if (value != null)
                {
                    return value;
                }
            }

            return Environment.GetEnvironmentVariable(key);
        }
    }
}