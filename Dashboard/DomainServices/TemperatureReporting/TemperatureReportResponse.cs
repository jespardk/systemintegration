namespace DomainServices.TemperatureReporting
{
    public class TemperatureReportResponse
    {
        public DateTime Date { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        public decimal DegreesCelcius { get; set; }

        public override string ToString()
        {
            return $"{Date} - {TimeOfDay} - {DegreesCelcius}";
        }
    }
}
