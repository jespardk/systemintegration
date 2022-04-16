using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Domain.PowerMeasurement;
using Domain.Configuration;

namespace Domain.DanishEnergyPrices
{
    public class DanishEnergyPriceRetriever
    {
        private const string _cacheKey = "DanishEnergyPrice.PriceCache";
        private const double CONVERSION_PRICE_DKK_TO_EUR = 7.4377;
        private string? _baseUrl;

        public DanishEnergyPriceRetriever(IConfiguration configuration)
        {
            var config = new ConfigurationRetriever(configuration);

            _baseUrl = config.GetConfigValue("DanishEnergyPrice.BaseUrl");
        }

        public async Task<DanishEnergyPriceResponse> GetDayPricesForPriceArea(DanishEnergyPriceArea area, int hoursToCollect = 24)
        {
            var response = new DanishEnergyPriceResponse
            {
                PriceArea = area,
                HourSpan = hoursToCollect,
                Records = new List<DanishEnergyPriceRecordResponse>(),
                DataSourceType = "WebApi (REST)"
            };

            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_baseUrl);

                var urlSegment = $"datastore_search_sql?sql=";
                var query = $"SELECT \"HourDK\", \"SpotPriceDKK\" from \"elspotprices\" WHERE \"PriceArea\" = '{area}' ORDER BY \"HourDK\" DESC LIMIT {hoursToCollect}";
                var result = await httpClient.GetStringAsync(urlSegment + query);
                var resultAsEnergiDataServiceDkResponse = JsonConvert.DeserializeObject<Rootobject>(result);
                response.Records = resultAsEnergiDataServiceDkResponse.result.records.Select(x => MapToDto(x)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PowerMeasurementRetriever)}: Read data from WebApi resource threw exception: {ex.Message}");
            }

            return response;
        }

        private static DanishEnergyPriceRecordResponse MapToDto(Record x)
        {
            double? price = x.SpotPriceDKK == null ? null : (double)x.SpotPriceDKK;
            var dto = new DanishEnergyPriceRecordResponse
            {
                HourDk = x.HourDK,
                SpotPriceMegawattInDKK = price.HasValue ? Math.Round(price.Value, 3) : null,
                SpotPriceKilowattInDKK = price.HasValue ? Math.Round(price.Value / 1000, 3) : null,
                SpotPriceMegawattInEUR = price.HasValue ? Math.Round(price.Value / DanishEnergyPriceRetriever.CONVERSION_PRICE_DKK_TO_EUR, 3) : null,
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