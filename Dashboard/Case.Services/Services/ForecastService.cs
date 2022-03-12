using Case.Services.Models;
using Case.Services.References.ForecastService;

namespace Case.Services
{
    public class ForecastService
    {
        public async Task<ForecastAggregateResponse> GetForecastAsync()
        {
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
                response.DataNext24Hours.Add(new ForecastResponse
                {
                    Hour = item.datetimeStr.Hour,
                    CloudCover = item.cloudcover.Value,
                    DegreesCelsius = item.temp.Value
                });
            }

            return response;
        }
    }
}