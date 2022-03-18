namespace Case.Services.Models
{
    public class TemperatureReportAggregateResponse : BaseResponse
    {
        public List<TemperatureReportResponse> Data { get; set; }

        public TemperatureReportAggregateResponse()
        {
            DataSourceType = "SQL server";
            Data = new List<TemperatureReportResponse>();
        }
    }
}
