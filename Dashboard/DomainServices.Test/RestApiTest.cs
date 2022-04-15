using DomainServices.PowerMeasurements;
using Common.Helpers;
using Xunit;
using DomainServices.DanishEnergyPrice;

namespace DomainServices.Test
{
    public class RestApiTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("DanishEnergyPrice.BaseUrl", "https://api.energidataservice.dk");

            var service = new DanishEnergyPriceRetriever(null);

            var prices = await service.GetDayPricesForPriceArea(DanishEnergyPriceArea.DK1, 72);
        }
    }
}
