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
            var service = new WeatherForecastRetriever(null);
            var result = service.GetForecastAsync().Result;
        }
    }
}
