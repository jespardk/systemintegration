using Domain.Configuration;
using Common.Helpers;
using System.Net;
using Domain.Caching;

namespace Domain.PowerMeasurement
{
    public class PowerMeasurementRetriever
    {
        private const string _cacheKey = "PowerMeasurements.WattsCache";
        private string? _username;
        private string? _password;
        private string? _url;
        private ICacheService _cacheService;

        public PowerMeasurementRetriever(IConfigurationRetriever configurationRetriever,
                                         ICacheService cacheService)
        {
            _cacheService = cacheService;
            
            _username = configurationRetriever.Get("PowerMeasurements.Username");
            _password = configurationRetriever.Get("PowerMeasurements.Password");
            _url = configurationRetriever.Get("PowerMeasurements.Url");
        }

        public async Task<PowerProductionResponse> GetMeasurementsAsync()
        {
            var response = new PowerProductionResponse();
            response.DataSourceType = "FTP/CSV";

            try
            {
                var cachedItem = _cacheService.Get<PowerProductionResponse>(_cacheKey);
                if (cachedItem != null)
                {
                    cachedItem.IsFromCache = true;
                    return cachedItem;
                }

                var filesOverview = GetSortedFileNameList();
                string? selectedFile = PickFileFromOneYearAgo(filesOverview);

                FtpWebResponse ftpResponse = await GetFromFtpSource(selectedFile);

                using (Stream responseStream = ftpResponse.GetResponseStream())
                {
                    using StreamReader streamReader = new StreamReader(responseStream);

                    var rawCsvSanitized = PowerMeasurementCsvSanitizer.Sanitize(streamReader.ReadToEnd());
                    var csvData = CsvConverter.Convert<PowerMeasurementCsvSchema>(rawCsvSanitized);
                    var selectedData = csvData?.OrderByDescending(_ => _.TIMESTAMP).FirstOrDefault();

                    response.DateTime = selectedData.TIMESTAMP;
                    response.Watts = selectedData.Current_Day_Energy;

                    Console.WriteLine($"{nameof(PowerMeasurementRetriever)}: Read data from FTP resource, status: {ftpResponse.StatusDescription}");

                    streamReader.Close();
                    responseStream.Close();
                }

                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PowerMeasurementRetriever)}: Read data from FTP resource threw exception: {ex.Message}");
            }

            _cacheService.Set(_cacheKey, response, 120);
            Console.WriteLine($"{GetType().Name}: Read data from FTP resource");

            return response;
        }

        private static string? PickFileFromOneYearAgo(List<string> allfiles)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1).ToString("yyMMdd");
            var selectedFile = allfiles?.FirstOrDefault(x => x.Replace("danfoss-", "")?.Substring(0, 6) == oneYearAgo);
            return selectedFile;
        }

        private async Task<FtpWebResponse> GetFromFtpSource(string filename)
        {
            if (!_url.StartsWith("ftp://")) throw new ArgumentException("Needs ftp:// prefix");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_url + "/" + filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            FtpWebResponse ftpResponse = (FtpWebResponse)await request.GetResponseAsync();

            return ftpResponse;
        }

        private List<string> GetSortedFileNameList()
        {
            try
            {
                FtpWebRequest request = WebRequest.Create(_url) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(_username, _password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string files = streamReader.ReadToEnd();

                streamReader.Close();
                response.Close();

                var filelist = files
                    .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .OrderByDescending(x => x)
                    .ToList();

                return filelist;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}