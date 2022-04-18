using Domain.Caching;
using Domain.Configuration;
using Domain.WeatherForecast;
using Xunit;

namespace Domain.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("WeatherForecast.AuthKey", "");

            var configurationRetriever = new ConfigurationRetriever(null);
            var cacheService = new CacheService();
            var service = new WeatherForecastRetriever(configurationRetriever, cacheService);
            var result = service.GetForecastAsync().Result;
        }
    }
}
