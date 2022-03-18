namespace Case.Services.Models
{
    public class PowerProductionResponse : BaseResponse
    {
        public float Watts { get; set; }

        public PowerProductionResponse()
        {
            DataSourceType = "FTP/CSV";
            Watts = -1;
        }
    }
}
