using Case.Services.Models;
using Case.Services.References.ForecastService;
using System.Runtime.Caching;

namespace Case.Services
{
    public class WeatherService
    {
        private const string _cacheKey = "ForecastService.ForecastCache";

        public async Task<ForecastAggregateResponse> GetForecastAsync()
        {
            var cachedItem = CacheService.MemoryCache.Get(_cacheKey) as ForecastAggregateResponse;
            if (cachedItem != null)
            {
                Console.WriteLine($"{nameof(WeatherService)}: Read data cached");
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var key = Environment.GetEnvironmentVariable("ForecastService.AuthKey");
            var client = new ForecastServiceClient();
            GetForecastResponse result = await client.GetForecastAsync("Aarhus", key);

            var tomorrow = DateTime.Now.AddDays(1);
            var next24HourData = result.Body.GetForecastResult.location.values.Where(_ => _.datetimeStr < tomorrow);

            var response = new ForecastAggregateResponse();
            response.FetchDateTime = DateTime.Now;
            response.LocationName = result.Body.GetForecastResult.location.name;
            response.DataNext24Hours = new List<ForecastResponse>();

            foreach (var item in next24HourData)
            {
                var windSpeedMS = (float)Math.Round((item.wspd.Value / 1.94384F), 1);

                response.DataNext24Hours.Add(new ForecastResponse
                {
                    Hour = item.datetimeStr.Hour,
                    CloudCover = item.cloudcover.Value,
                    DegreesCelsius = item.temp.Value,
                    WindSpeedMeterPrSecond = windSpeedMS,
                });
            }

            // Cache result
            CacheService.MemoryCache.Set(_cacheKey, response, DateTimeOffset.Now.AddSeconds(60));
            Console.WriteLine($"{nameof(WeatherService)}: Read data from SOAP resource");

            return response;
        }
    }
}