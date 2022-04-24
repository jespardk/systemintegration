using Common.Serialization;
using Domain.Caching;
using Newtonsoft.Json;

namespace Domain.DanishEnergyPrices
{
    public class IncomingDanishEnergyPriceHandler
    {
        private readonly ICacheService _cacheService;

        public IncomingDanishEnergyPriceHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void OnNewPricesReceived(string message)
        {
            // Receives prices
            var today = DateTime.Now.Date;
            var DayInSeconds = 60 * 60 * 24;

            try
            {
                var converted = JsonConvert.DeserializeObject<DanishEnergyPriceResponse>(message, JsonSerializerSettingsProvider.WithSilentErrorHandling());

                if (converted != null /*&& converted.DateTime.Date != today*/)
                {
                    // Cache prices
                    _cacheService.Set(DanishEnergyPriceRetriever.CacheKeyAllDay, converted, DayInSeconds);
                    Console.WriteLine($"{nameof(IncomingDanishEnergyPriceHandler)}: Stored in cache! Try the dashboard again");
                }
                else
                {
                    Console.WriteLine($"{nameof(IncomingDanishEnergyPriceHandler)}: Ignoring unrecognized format...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(IncomingDanishEnergyPriceHandler)}: Error: " + ex.Message);
            }
        }
    }
}