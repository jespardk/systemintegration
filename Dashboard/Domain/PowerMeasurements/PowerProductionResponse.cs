using Common.Models;

namespace Domain.PowerMeasurements
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
