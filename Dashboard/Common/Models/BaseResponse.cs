namespace Common.Models
{
    public class BaseResponse
    {
        public bool RequestSuccessful { get; set; }
        public DateTime DateTime { get; set; }
        public string? DataSourceType { get; set; }
        public bool IsFromCache { get; set; }

        public BaseResponse()
        {
            DateTime = DateTime.Now;
        }
    }
}
