using DomainServices.WeatherForecast;
using Xunit;

namespace DomainServices.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("ForecastService.AuthKey", "CHANGEME!");
            var service = new WeatherService(null);
            var result = service.GetForecastAsync().Result;
        }
    }
}
