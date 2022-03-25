using Case.Services.Models;
using Case.Services.References.ForecastService;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Case.Services
{
    public class TemperatureReportingService
    {
        private const string _cacheKey = "TemperatureReportingService.Cache";
        private string _connectionString;

        public TemperatureReportingService(IConfiguration configuration)
        {
            var configService = new ConfigurationService(configuration);
            _connectionString = configService.GetConfigValue("TemperatureReportingService.ConnectionString");
        }

        public TemperatureReportAggregateResponse GetTemperatureRecent()
        {
            var cachedItem = CacheService.MemoryCache?.Get(_cacheKey) as TemperatureReportAggregateResponse;
            if (cachedItem != null)
            {
                Console.WriteLine($"{GetType().Name}: Read data cached");
                cachedItem.IsFromCache = true;
                return cachedItem;
            }

            var response = new TemperatureReportAggregateResponse();
            response.DataSourceType = "SQL server";
            response.DateTime = DateTime.Now;

            try
            {
                var collectedData = new List<TemperatureReportResponse>();

                using var connection = new SqlConnection(_connectionString);

                connection.Open();

                String sql = 
                    "SELECT TOP(10) [dato],[tidspunkt],[grader] " +
                    "FROM [dbo].[Temperatur] " +
                    "ORDER BY [dato] DESC";

                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new TemperatureReportResponse
                    {
                        Date = reader.GetDateTime(0),
                        TimeOfDay = reader.GetTimeSpan(1),
                        DegreesCelcius = reader.GetDecimal(2)
                    };

                    response.Data.Add(item);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            // Cache result
            CacheService.MemoryCache?.Set(_cacheKey, response, DateTimeOffset.Now.AddSeconds(60));
            Console.WriteLine($"{GetType().Name}: Read data from SQL source");

            return response;
        }
    }
}