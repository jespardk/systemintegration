﻿using Case.Services.Models;
using Case.Services.References.ForecastService;
using Microsoft.Extensions.Configuration;

namespace Case.Services
{
    public class WeatherService
    {
        private const string _cacheKey = "ForecastService.ForecastCache";
        private string _key;

        public WeatherService(IConfiguration configuration)
        {
            var configService = new ConfigurationService(configuration);
            _key = configService.GetConfigValue("ForecastService.AuthKey");
        }

        public async Task<ForecastAggregateResponse> GetForecastAsync()
        {
            var cachedItem = CacheService.MemoryCache.Get(_cacheKey) as ForecastAggregateResponse;
            if (cachedItem != null)
            {
                Console.WriteLine($"{nameof(WeatherService)}: Read data cached");
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var client = new ForecastServiceClient();
            GetForecastResponse result = await client.GetForecastAsync("Aarhus", _key);

            var dateNowPlus12Hours = DateTime.Now.AddHours(9);
            var relevantForecastData = result.Body.GetForecastResult.location.values.Where(_ => _.datetimeStr < dateNowPlus12Hours);

            var response = new ForecastAggregateResponse();
            response.DataSourceType = "SOAP service";
            response.DateTime = DateTime.Now;
            response.LocationName = result.Body.GetForecastResult.location.name;
            response.Data = new List<ForecastResponse>();

            foreach (var item in relevantForecastData)
            {
                var windSpeedMS = (float)Math.Round((item.wspd.Value / 1.94384F), 1);

                response.Data.Add(new ForecastResponse
                {
                    Hour = item.datetimeStr.Hour,
                    CloudCover = item.cloudcover.Value,
                    DegreesCelsius = item.temp.Value,
                    WindSpeedMeterPrSecond = windSpeedMS,
                });
            }

            // Cache result
            CacheService.MemoryCache.Set(_cacheKey, response, DateTimeOffset.Now.AddSeconds(120));
            Console.WriteLine($"{nameof(WeatherService)}: Read data from SOAP resource");

            return response;
        }
    }
}