using Case.Services.Helpers;
using Case.Services.Models;
using Case.Services.Schema;
using System.Net;

namespace Case.Services
{
    public class PowerMeasurementsService
    {
        private const string _cacheKey = "PowerMeasurementsService.WattsCache";

        public async Task<PowerProductionResponse> GetMeasurementsAsync()
        {
            var response = new PowerProductionResponse();

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
                    StreamReader streamReader = new StreamReader(responseStream);

                    var csvData = CsvHandler.ConvertCsv<PowermeasurementCsvSchema>(streamReader);

                    var value = streamReader.ReadToEnd();

                    Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource, status: {ftpResponse.StatusDescription}");

                    streamReader.Close();
                }

                ftpResponse.Close();

                // TODO Convert CSV schema classes to actual output that can be used on the dashboard
                // ....add to response before returning
                response.Watts = 1231;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource threw exception: {ex.Message}");
            }

            //CacheService.MemoryCache.Set(_cacheKey, response, DateTimeOffset.Now.AddSeconds(120));
            //Console.WriteLine($"{nameof(WeatherService)}: Read data from FTP resource");

            return response;
        }

        private static async Task<FtpWebResponse> GetFromFtpSource(string filename)
        {
            var username = Environment.GetEnvironmentVariable("PowerMeasurementsService.Username");
            var password = Environment.GetEnvironmentVariable("PowerMeasurementsService.Password");
            var url = Environment.GetEnvironmentVariable("PowerMeasurementsService.Url");

            if (!url.StartsWith("ftp://")) throw new ArgumentException("Needs ftp:// prefix");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url + "/" + filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);
            //request.EnableSsl = true;

            FtpWebResponse ftpResponse = (FtpWebResponse)await request.GetResponseAsync();

            return ftpResponse;
        }

        private List<string> ListAndSortFilenames()
        {
            try
            {
                var username = Environment.GetEnvironmentVariable("PowerMeasurementsService.Username");
                var password = Environment.GetEnvironmentVariable("PowerMeasurementsService.Password");
                var url = Environment.GetEnvironmentVariable("PowerMeasurementsService.Url");

                FtpWebRequest request = (FtpWebRequest) WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);

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