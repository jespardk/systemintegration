namespace Common.Models
{
    public class BaseResponse
    {
        public DateTime DateTime { get; set; }
        public string? DataSourceType { get; set; }
        public bool IsFromCache { get; set; }

        public BaseResponse()
        {
            DateTime = DateTime.Now;
        }
    }
}
