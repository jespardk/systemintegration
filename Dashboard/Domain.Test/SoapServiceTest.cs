using Domain.Caching;
using Domain.WeatherForecast;
using Xunit;

namespace Domain.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("ForecastService.AuthKey", "CHANGEME!");

            var cacheService = new CacheService();
            var service = new WeatherForecastRetriever(null, cacheService);
            var result = service.GetForecastAsync().Result;
        }
    }
}
