using Xunit;

namespace Case.Services.Test
{
    public class SoapServiceTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("ForecastService.AuthKey", "CHANGEME!");
            var service = new ForecastService();
            var result = service.GetForecastAsync().Result;
        }
    }
}
