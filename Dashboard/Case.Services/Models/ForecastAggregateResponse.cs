namespace Case.Services.Models
{
    public class ForecastAggregateResponse
    {
        public string LocationName { get; set; }
        public List<ForecastResponse> DataNext24Hours { get; set; }
        public DateTime FetchDateTime { get; set; }
        public bool IsFromCache { get; set; }
    }
}
