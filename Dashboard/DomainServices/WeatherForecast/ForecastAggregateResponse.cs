using Common.Models;

namespace DomainServices.WeatherForecast
{
    public class ForecastAggregateResponse : BaseResponse
    {
        public string? LocationName { get; set; }
        public List<ForecastResponse>? Data { get; set; }
    }
}
