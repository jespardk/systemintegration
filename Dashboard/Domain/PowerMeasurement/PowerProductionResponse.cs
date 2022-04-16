using Common.Models;

namespace Domain.PowerMeasurement
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
