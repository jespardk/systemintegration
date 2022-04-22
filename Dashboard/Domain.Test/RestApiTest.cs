
using Xunit;
using Domain.DanishEnergyPrices;
using Infrastructure.Configuration;
using Infrastructure.Caching;

namespace Domain.Test
{
    public class RestApiTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("DanishEnergyPrice.ApiBaseUrl", "https://api.energidataservice.dk");

            var configurationRetriever = new ConfigurationRetriever(null);
            var cacheService = new InMemoryCache();
            var service = new DanishEnergyPriceRetriever(configurationRetriever, cacheService);

            var prices = await service.GetDayPricesForPriceAreaAsync(DanishEnergyPriceArea.DK1, 96);
            var withPriceData = prices.RecordsWithPriceData;
        }
    }
}
