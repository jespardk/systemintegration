namespace Case.Services.Models
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
