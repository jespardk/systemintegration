using Domain.Configuration;
using Domain.WeatherForecast.ServiceReference;
using Domain.Caching;
using Polly;
using Polly.Retry;

namespace Domain.WeatherForecast
{
    public class WeatherForecastRetriever
    {
        private readonly ICacheService _cacheService;
        private readonly ForecastServiceClient _client;
        private const string CacheKey = "WeatherForecast.ForecastCache";
        private string? _key;
        private string? _location = "Kolding";
        private AsyncRetryPolicy _retryPolicy = null;

        public WeatherForecastRetriever(IConfigurationRetriever configurationRetriever, ICacheService cacheService)
        {
            _key = configurationRetriever.Get("WeatherForecast.AuthKey");
            _location = configurationRetriever.Get("WeatherForecast.Location");
            _cacheService = cacheService;

            _client = new ForecastServiceClient();
            _client.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(5);

            SetupRetryingPolicy();
        }

        public async Task<ForecastAggregateResponse> GetForecastAsync()
        {
            var response = new ForecastAggregateResponse();
            response.DataSourceType = "SOAP service";

            try
            {
                var cachedItem = _cacheService.Get<ForecastAggregateResponse>(CacheKey);
                if (cachedItem != null)
                {
                    cachedItem.IsFromCache = true;
                    return cachedItem;
                }

                GetForecastResponse result = null;

                await _retryPolicy.ExecuteAsync(async () =>
                {
                    result = await _client.GetForecastAsync(_location, _key);
                });

                var dateNowPlus12Hours = DateTime.Now.AddHours(9);
                var relevantForecastData = result.Body.GetForecastResult.location.values.Where(_ => _.datetimeStr < dateNowPlus12Hours);

                response.DateTime = DateTime.Now;
                response.LocationName = result.Body.GetForecastResult.location.name;
                response.Data = new List<ForecastResponse>();
                response.Data = relevantForecastData.Select(x => Map(x)).ToList();
                response.RequestSuccessful = true;

                // Cache result
                _cacheService.Set(CacheKey, response, 120);
                Console.WriteLine($"{GetType().Name}: Read data from SOAP resource");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from resource. Message {ex.Message}");
            }

            return response;
        }

        private static ForecastResponse Map(Value item)
        {
            var windSpeedMS = item.wspd.HasValue
                                    ? (float)Math.Round(item.wspd.Value / 1.94384F, 1)
                                    : 0;

            return new ForecastResponse
            {
                Hour = item.datetimeStr.Hour,
                CloudCover = item.cloudcover.HasValue ? item.cloudcover.Value : 0,
                DegreesCelsius = item.temp.HasValue ? item.temp.Value : 0,
                WindSpeedMeterPrSecond = windSpeedMS,
            };
        }

        private void SetupRetryingPolicy()
        {
            var retrySleepDuration = TimeSpan.FromSeconds(3);

            _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                   retryCount: 3,
                   sleepDurationProvider: (x) => retrySleepDuration,
                   onRetry: (exception, sleepDuration, attemptNumber, context) =>
                   {
                       Console.WriteLine($"{GetType().Name} Error during service communication. Retrying in {retrySleepDuration}. {attemptNumber} / {3}");
                   });
        }
    }
}