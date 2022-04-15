using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Domain.PowerMeasurements;
using Domain.Configuration;

namespace Domain.DanishEnergyPrice
{
    public class DanishEnergyPriceRetriever
    {
        private const string _cacheKey = "DanishEnergyPrice.PriceCache";
        private string? _baseUrl;

        public DanishEnergyPriceRetriever(IConfiguration configuration)
        {
            var config = new ConfigurationService(configuration);

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
                Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from WebApi resource threw exception: {ex.Message}");
            }

            return response;
        }

        private static DanishEnergyPriceRecordResponse MapToDto(Record x)
        {
            double? price = x.SpotPriceDKK != null ? (double)x.SpotPriceDKK : null;

            return new DanishEnergyPriceRecordResponse
            {
                HourDk = x.HourDK,
                SpotPriceMegawattInDKK = price.HasValue ? Math.Round(price.Value, 3) : null,
                SpotPriceKilowattInDKK = price.HasValue ? Math.Round(price.Value / 1000, 3) : null,
                SpotPriceMegawattInEUR = price.HasValue ? Math.Round(price.Value / 7.4377, 3) : null,
                SpotPriceKilowattInEUR = price.HasValue ? Math.Round(price.Value / 1000 / 7.4377, 3) : null
            };
        }
    }
}