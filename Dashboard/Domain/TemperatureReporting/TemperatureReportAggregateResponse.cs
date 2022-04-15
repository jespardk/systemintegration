using Common.Models;

namespace Domain.TemperatureReporting
{
    public class TemperatureReportAggregateResponse : BaseResponse
    {
        public List<TemperatureReportResponse> Data { get; set; }

        public TemperatureReportAggregateResponse()
        {
            Data = new List<TemperatureReportResponse>();
        }
    }
}
