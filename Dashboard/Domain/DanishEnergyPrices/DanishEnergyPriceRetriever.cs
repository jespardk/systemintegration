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

        public DanishEnergyPriceRetriever(IConfigurationRetriever configurationRetriever)
        {
            _baseUrl = configurationRetriever.Get("DanishEnergyPrice.ApiBaseUrl");
        }

        public async Task<DanishEnergyPriceResponse> GetDayPricesForPriceAreaAsync(DanishEnergyPriceArea area, int hoursToCollect = 24)
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
                var query = $"SELECT \"HourDK\", \"SpotPriceDKK\", \"SpotPriceEUR\" from \"elspotprices\" WHERE \"PriceArea\" = '{area}' ORDER BY \"HourDK\" DESC LIMIT {hoursToCollect}";
                var result = await httpClient.GetStringAsync(urlSegment + query);
                var resultAsEnergiDataServiceDkResponse = JsonConvert.DeserializeObject<Rootobject>(result);

                response.Records = resultAsEnergiDataServiceDkResponse.result.records
                    .Select(x => MapToDto(x))
                    .OrderBy(_ => _.HourDk)
                    .ToList();

                response.RequestSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}: Read data from WebApi resource threw exception: {ex.Message}");
            }

            return response;
        }

        private static DanishEnergyPriceRecordResponse MapToDto(Record x)
        {
            double? price = x.SpotPriceDKK != null 
                ? (double)x.SpotPriceDKK 
                : null;

            if (price == null && x.SpotPriceEUR != null)
            {
                // In case only EUR price is available, convert this to danish
                price = (double)x.SpotPriceEUR * CONVERSION_PRICE_DKK_TO_EUR;
            }

            var dto = new DanishEnergyPriceRecordResponse
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