using Common.Models;

namespace Domain.WeatherForecast
{
    public class ForecastAggregateResponse : BaseResponse
    {
        public string? LocationName { get; set; }
        public List<ForecastResponse>? Data { get; set; }
    }
}
