
using Xunit;
using Domain.DanishEnergyPrices;
using Domain.Configuration;
using Infrastructure.Configuration;

namespace Domain.Test
{
    public class RestApiTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("DanishEnergyPrice.ApiBaseUrl", "https://api.energidataservice.dk");

            var configurationRetriever = new ConfigurationRetriever(null);
            var service = new DanishEnergyPriceRetriever(configurationRetriever);

            var prices = await service.GetDayPricesForPriceAreaAsync(DanishEnergyPriceArea.DK1, 96);
            var withPriceData = prices.RecordsWithPriceData;
        }
    }
}
