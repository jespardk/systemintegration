namespace Case.Services.Models
{
    public class TemperatureReportAggregateResponse
    {
        public DateTime DateTime { get; set; }
        public List<TemperatureReportResponse> Data { get; set; }
        public bool IsFromCache { get; set; }

        public TemperatureReportAggregateResponse()
        {
            Data = new List<TemperatureReportResponse>();
        }
    }
}
