namespace Case.Services.Models
{
    public class PowerProductionResponse
    {
        public DateTime DateTime { get; set; }
        public float Watts { get; set; }
        public bool IsFromCache { get; set; }

        public PowerProductionResponse()
        {
            Watts = -1;
        }
    }
}
