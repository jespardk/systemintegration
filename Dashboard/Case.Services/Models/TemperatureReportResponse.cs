namespace Case.Services.Models
{
    public class TemperatureReportResponse
    {
        public DateTime dato { get; set; }
        public TimeSpan tidspunkt { get; set; }
        public decimal grader { get; set; }
        public bool IsFromCache { get; set; }

        public TemperatureReportResponse()
        {
        }

        public override string ToString()
        {
            return $"{dato} - {tidspunkt} - {grader}";
        }
    }
}
