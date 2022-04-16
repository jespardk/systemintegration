﻿
using Xunit;
using Domain.DanishEnergyPrices;

namespace Domain.Test
{
    public class RestApiTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("DanishEnergyPrice.BaseUrl", "https://api.energidataservice.dk");

            var service = new DanishEnergyPriceRetriever(null);

            var prices = await service.GetDayPricesForPriceArea(DanishEnergyPriceArea.DK1, 96);
            var withPriceData = prices.RecordsWithPriceData;
        }
    }
}
