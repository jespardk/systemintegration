namespace Client.ViewModels
{
    public class WeatherPriceAggregateViewModel
    {
        public DateTime DateTime { get; set; }
        public DateTime Hour { get; set; }
        public double PriceInDkkPrKwH { get; set; }
        public double ForecastDegreesC { get; set; }
        public double ForecastCloudcoverPercentage { get; set; }
    }
}
