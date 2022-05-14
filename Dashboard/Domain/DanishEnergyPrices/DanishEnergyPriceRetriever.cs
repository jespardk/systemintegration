using Newtonsoft.Json;
using Domain.Configuration;
using Domain.Caching;
using Common.ResiliencyPolicies;
using Domain.DanishEnergyPrices.EnergiDataServiceApi;
using Common.Serialization;

namespace Domain.DanishEnergyPrices
{
    public class DanishEnergyPriceRetriever
    {
        public const string CacheKeyAllDay = "DanishEnergyPrice.PriceCacheAllDay";

        private string? _baseUrl;
        private const string _cacheKey = "DanishEnergyPrice.PriceCache";
        private const double CONVERSION_PRICE_DKK_TO_EUR = 7.4377;
        private readonly ICacheService _cacheService;

        public DanishEnergyPriceRetriever(IConfigurationRetriever configurationRetriever, ICacheService cacheService)
        {
            _baseUrl = configurationRetriever.Get("DanishEnergyPrice.ApiBaseUrl");
            _cacheService = cacheService;
        }

        public DanishEnergyPriceResponse GetPricesFromDayCacheAsync()
        {
            var cachedItem = _cacheService.Get<DanishEnergyPriceResponse>(CacheKeyAllDay);
            if (cachedItem != null)
            {
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var emptyResponse = new DanishEnergyPriceResponse
            {
                IsFromCache = false,
                RequestSuccessful = true
            };

            return emptyResponse;
        }

        public async Task<DanishEnergyPriceResponse> GetDayPricesForPriceAreaAsync(DanishEnergyPriceArea area, int hoursToCollect = 24)
        {
            var cachedItem = _cacheService.Get<DanishEnergyPriceResponse>(_cacheKey);
            if (cachedItem != null)
            {
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var response = new DanishEnergyPriceResponse
            {
                PriceArea = area,
                HourSpan = hoursToCollect,
                Records = new List<DanishEnergyPriceRecord>(),
                DataSourceType = "WebApi (REST)"
            };

            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_baseUrl);

                var urlSegment = $"datastore_search_sql?sql=";
                var query = $"SELECT \"HourDK\", \"SpotPriceDKK\", \"SpotPriceEUR\" from \"elspotprices\" WHERE \"PriceArea\" = '{area}' ORDER BY \"HourDK\" DESC LIMIT {hoursToCollect}";

                var retryPolicy = new RetryingPolicyBuilder<DanishEnergyPriceRetriever>()
                    .WithDelay(3)
                    .WithRetries(3)
                    .Build();

                string? result = null;
                await retryPolicy.ExecuteAsync(async () =>
                {
                    result = await httpClient.GetStringAsync(urlSegment + query);
                });

                var resultAsEnergiDataServiceDkResponse = JsonConvert.DeserializeObject<Rootobject>(result, JsonSerializerSettingsProvider.WithSilentErrorHandling());
                if (resultAsEnergiDataServiceDkResponse != null)
                {
                    response.Records = resultAsEnergiDataServiceDkResponse.result.records
                        .Select(MapToDto)
                        .OrderBy(_ => _.HourDk)
                        .ToList();

                    response.RequestSuccessful = true;

                    // Cache result
                    _cacheService.Set(_cacheKey, response, 120);
                    Console.WriteLine($"{GetType().Name}: Read data from {response.DataSourceType} resource");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}: Read data from WebApi resource threw exception: {ex.Message}");
            }

            return response;
        }

        private static DanishEnergyPriceRecord MapToDto(Record x)
        {
            double? price = x.SpotPriceDKK != null
                ? (double)x.SpotPriceDKK
                : null;

            if (price == null && x.SpotPriceEUR != null)
            {
                // In case only EUR price is available, convert this to danish
                price = (double)x.SpotPriceEUR * CONVERSION_PRICE_DKK_TO_EUR;
            }

            var dto = new DanishEnergyPriceRecord
            {
                HourDk = x.HourDK,
                SpotPriceMegawattInDKK = price.HasValue ? Math.Round(price.Value, 3) : null,
                SpotPriceKilowattInDKK = price.HasValue ? Math.Round(price.Value / 1000, 3) : null,
                SpotPriceMegawattInEUR = price.HasValue ? Math.Round(price.Value / CONVERSION_PRICE_DKK_TO_EUR, 3) : null,
                SpotPriceKilowattInEUR = price.HasValue ? Math.Round((price.Value / 1000) / CONVERSION_PRICE_DKK_TO_EUR, 3) : null
            };

            if (price.HasValue)
            {
                dto.HasPriceData = true;
            }

            return dto;
        }
    }
}