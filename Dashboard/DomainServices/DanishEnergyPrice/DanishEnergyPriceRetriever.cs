using DomainServices.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DomainServices.DanishEnergyPrice;

namespace DomainServices.PowerMeasurements
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

        public async Task<DanishEnergyPriceResponse> GetDayPricesForPriceArea(string area, int hoursToCollect = 24)
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

                var segment = $"datastore_search_sql?sql=SELECT \"HourDK\", \"SpotPriceDKK\" from \"elspotprices\" WHERE \"PriceArea\" = '{area}' ORDER BY \"HourDK\" DESC LIMIT {hoursToCollect}";
                var result = await httpClient.GetStringAsync(segment);
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
            double price = x.SpotPriceDKK != null ? (double)x.SpotPriceDKK : 0;

            return new DanishEnergyPriceRecordResponse
            {
                HourDk = x.HourDK,
                SpotPriceMegawattInDKK = Math.Round(price, 3),
                SpotPriceKilowattInDKK = price != 0 ? Math.Round((price / 1000), 3) : 0,
                SpotPriceMegawattInEUR = price != 0 ? Math.Round((price / 7.4377), 3) : 0,
                SpotPriceKilowattInEUR = price != 0 ? Math.Round(((price/1000) / 7.4377), 3) : 0
            };
        }
    }
}