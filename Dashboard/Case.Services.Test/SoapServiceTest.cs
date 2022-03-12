using Xunit;

namespace Case.Services.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("ForecastService.AuthKey", "CHANGEME!");
            var service = new WeatherService();
            var result = service.GetForecastAsync().Result;
        }
    }
}
