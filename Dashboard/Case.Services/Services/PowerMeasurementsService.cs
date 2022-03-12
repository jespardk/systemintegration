using Case.Services.Helpers;
using Case.Services.Models;
using Case.Services.Schema;
using System.Net;

namespace Case.Services
{
    public class PowerMeasurementsService
    {
        public PowerProductionResponse GetMeasurements()
        {
            var username = Environment.GetEnvironmentVariable("PowerMeasurementsService.Username");
            var password = Environment.GetEnvironmentVariable("PowerMeasurementsService.Password");
            var url = Environment.GetEnvironmentVariable("PowerMeasurementsService.Url");

            if (!url.StartsWith("ftp://")) throw new ArgumentException("Needs ftp:// prefix");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.EnableSsl = true;

            var response = new PowerProductionResponse();

            FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();

            using (Stream responseStream = ftpResponse.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(responseStream);

                var csvData = CsvHandler.ConvertCsv<PowermeasurementCsvSchema>(streamReader);

                Console.WriteLine($"Download Complete, status {ftpResponse.StatusDescription}");

                streamReader.Close();
            }

            ftpResponse.Close();

            // TODO Convert CSV schema classes to actual output that can be used on the dashboard
            // ....add to response before returning

            return new PowerProductionResponse();
        }
    }
}