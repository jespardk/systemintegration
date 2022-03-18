namespace Case.Services.Models
{
    public class TemperatureReportResponse
    {
        public DateTime dato { get; set; }
        public TimeSpan tidspunkt { get; set; }
        public decimal grader { get; set; }

        public override string ToString()
        {
            return $"{dato} - {tidspunkt} - {grader}";
        }
    }
}
