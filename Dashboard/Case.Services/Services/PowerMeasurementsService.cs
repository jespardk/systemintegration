using Case.Services.Helpers;
using Case.Services.Models;
using Case.Services.Schema;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Case.Services
{
    public class PowerMeasurementsService
    {
        private const string _cacheKey = "PowerMeasurementsService.WattsCache";
        private string? _username;
        private string? _password;
        private string? _url;

        public PowerMeasurementsService(IConfiguration configuration)
        {
            var config = new ConfigurationService(configuration);

            _username = config.GetConfigValue("PowerMeasurementsService.Username");
            _password = config.GetConfigValue("PowerMeasurementsService.Password");
            _url = config.GetConfigValue("PowerMeasurementsService.Url");
        }

        public async Task<PowerProductionResponse> GetMeasurementsAsync()
        {
            var response = new PowerProductionResponse();
            response.DataSourceType = "FTP/CSV";

            try
            {
                var cachedItem = CacheService.MemoryCache.Get(_cacheKey) as PowerProductionResponse;
                if (cachedItem != null)
                {
                    Console.WriteLine($"{nameof(WeatherService)}: Read data cached");
                    cachedItem.IsFromCache = true;
                    return cachedItem;
                }

                var allfiles = ListAndSortFilenames();

                FtpWebResponse ftpResponse = await GetFromFtpSource(allfiles.FirstOrDefault());
                                
                using (Stream responseStream = ftpResponse.GetResponseStream())
                {
                    using StreamReader streamReader = new StreamReader(responseStream);

                    var csvData = CsvHandler.ConvertCsvV2<PowermeasurementCsvSchema>(streamReader.ReadToEnd());
                    var selectedData = csvData?.OrderByDescending(_ => _.TIMESTAMP)?.FirstOrDefault();

                    response.DateTime = selectedData.TIMESTAMP;
                    response.Watts = selectedData.Current_Day_Energy;

                    Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource, status: {ftpResponse.StatusDescription}");

                    streamReader.Close();
                    responseStream.Close();
                }

                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource threw exception: {ex.Message}");
            }

            CacheService.MemoryCache.Set(_cacheKey, response, DateTimeOffset.Now.AddSeconds(120));
            Console.WriteLine($"{nameof(WeatherService)}: Read data from FTP resource");

            return response;
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

        private List<string> ListAndSortFilenames()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_url);
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