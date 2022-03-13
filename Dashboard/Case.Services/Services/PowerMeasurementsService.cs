﻿using Case.Services.Helpers;
using Case.Services.Models;
using Case.Services.Schema;
using System.Net;

namespace Case.Services
{
    public class PowerMeasurementsService
    {
        public async Task<PowerProductionResponse> GetMeasurementsAsync()
        {
            var response = new PowerProductionResponse();

            try
            {
                FtpWebResponse ftpResponse = await GetFromFtpSource();

                using (Stream responseStream = ftpResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(responseStream);

                    var csvData = CsvHandler.ConvertCsv<PowermeasurementCsvSchema>(streamReader);

                    //var value = streamReader.ReadToEnd();

                    Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource, status: {ftpResponse.StatusDescription}");

                    streamReader.Close();
                }

                // TODO Convert CSV schema classes to actual output that can be used on the dashboard
                // ....add to response before returning
                response.Watts = 1231;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PowerMeasurementsService)}: Read data from FTP resource threw exception: {ex.Message}");
            }

            return response;
        }

        private static async Task<FtpWebResponse> GetFromFtpSource()
        {
            var username = Environment.GetEnvironmentVariable("PowerMeasurementsService.Username");
            var password = Environment.GetEnvironmentVariable("PowerMeasurementsService.Password");
            var url = Environment.GetEnvironmentVariable("PowerMeasurementsService.Url");

            if (!url.StartsWith("ftp://")) throw new ArgumentException("Needs ftp:// prefix");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);
            //request.EnableSsl = true;

            FtpWebResponse ftpResponse = (FtpWebResponse)await request.GetResponseAsync();
            ftpResponse.Close();

            return ftpResponse;
        }
    }
}