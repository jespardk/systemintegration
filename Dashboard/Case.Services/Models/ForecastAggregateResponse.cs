namespace Case.Services.Models
{
    public class ForecastAggregateResponse : BaseResponse
    {
        public string LocationName { get; set; }
        public List<ForecastResponse> Data { get; set; }

        public ForecastAggregateResponse()
        {
            DataSourceType = "SOAP service";
        }
    }
}
