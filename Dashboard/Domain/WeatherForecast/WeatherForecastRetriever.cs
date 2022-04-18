using Domain.Configuration;
using Domain.WeatherForecast.ServiceReference;
using Domain.Caching;

namespace Domain.WeatherForecast
{
    public class WeatherForecastRetriever
    {
        private readonly ICacheService _cacheService;
        private const string CacheKey = "WeatherForecast.ForecastCache";
        private string? _key;
        private string? _location = "Kolding";

        public WeatherForecastRetriever(IConfigurationRetriever configurationRetriever, ICacheService cacheService)
        {
            _key = configurationRetriever.Get("WeatherForecast.AuthKey");
            _location = configurationRetriever.Get("WeatherForecast.Location");
            _cacheService = cacheService;
        }

        public async Task<ForecastAggregateResponse> GetForecastAsync()
        {
            var cachedItem = _cacheService.Get<ForecastAggregateResponse>(CacheKey);
            if (cachedItem != null)
            {
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var client = new ForecastServiceClient();
            GetForecastResponse result = await client.GetForecastAsync(_location, _key);

            var dateNowPlus12Hours = DateTime.Now.AddHours(9);
            var relevantForecastData = result.Body.GetForecastResult.location.values.Where(_ => _.datetimeStr < dateNowPlus12Hours);

            var response = new ForecastAggregateResponse();
            response.DataSourceType = "SOAP service";
            response.DateTime = DateTime.Now;
            response.LocationName = result.Body.GetForecastResult.location.name;
            response.Data = new List<ForecastResponse>();

            foreach (var item in relevantForecastData)
            {
                var windSpeedMS = item.wspd.HasValue
                    ? (float)Math.Round(item.wspd.Value / 1.94384F, 1)
                    : 0;

                response.Data.Add(new ForecastResponse
                {
                    Hour = item.datetimeStr.Hour,
                    CloudCover = item.cloudcover.HasValue ? item.cloudcover.Value : 0,
                    DegreesCelsius = item.temp.HasValue ? item.temp.Value : 0,
                    WindSpeedMeterPrSecond = windSpeedMS,
                });
            }

            // Cache result
            _cacheService.Set(CacheKey, response, 120);
            Console.WriteLine($"{GetType().Name}: Read data from SOAP resource");

            return response;
        }
    }
}