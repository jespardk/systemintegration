namespace DomainServices.WeatherForecast
{
    public class ForecastResponse
    {
        public int Hour { get; set; }
        public float CloudCover { get; set; }
        public float DegreesCelsius { get; set; }
        public float WindSpeedMeterPrSecond { get; set; }
    }
}
