using Xunit;

namespace Case.Services.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("SoapReference.Forecast.AuthKey", "CHANGEME!");
            var service = new ForecastService();
            service.GetForecast();
        }
    }
}
