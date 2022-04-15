using DomainServices.PowerMeasurements;
using Common.Helpers;
using Xunit;

namespace DomainServices.Test
{
    public class RestApiTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("DanishEnergyPrice.BaseUrl", "https://api.energidataservice.dk");

            var service = new DanishEnergyPriceRetriever(null);

            var response = await service.GetDayPricesForPriceArea("DK1", 48);
        }
    }
}
