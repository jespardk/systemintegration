using Xunit;

namespace Case.Services.Test
{
    public class FtpTest
    {
        [Fact]
        public void TryService()
        {
            Environment.SetEnvironmentVariable("PowerMeasurementsService.Username", "");
            Environment.SetEnvironmentVariable("PowerMeasurementsService.Password", "");
            Environment.SetEnvironmentVariable("PowerMeasurementsService.Url", "ftp://CHANGEHOSTHERE/sometestdata.csv");

            var service = new PowerMeasurementsService();
            service.GetMeasurements();
        }
    }
}
