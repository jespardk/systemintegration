using Common.Models;

namespace DomainServices.PowerMeasurements
{
    public class PowerProductionResponse : BaseResponse
    {
        public float Watts { get; set; }

        public PowerProductionResponse()
        {
            Watts = -1;
        }
    }
}
