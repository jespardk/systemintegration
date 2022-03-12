using Case.Services.Models;
using Case.Services.References.ForecastService;

namespace Case.Services
{
    public class ForecastService
    {
        public ForecastAggregateResponse GetForecast()
        {
            var key = Environment.GetEnvironmentVariable("SoapReference.Forecast.AuthKey");
            var client = new ForecastServiceClient();
            GetForecastResponse result = client.GetForecastAsync("Aarhus", key).Result;

            var tomorrow = DateTime.Now.AddDays(1);
            var next24HourData = result.Body.GetForecastResult.location.values.Where(_ => _.datetimeStr < tomorrow);

            var response = new ForecastAggregateResponse();
            response.LocationName = result.Body.GetForecastResult.location.name;
            response.DataNext24Hours = new List<ForecastResponse>();

            foreach (var item in next24HourData)
            {
                response.DataNext24Hours.Add(new ForecastResponse
                {
                    CloudCover = item.cloudcover.Value,
                    DegreesCelsius = item.temp.Value
                });
            }

            return response;
        }
    }
}