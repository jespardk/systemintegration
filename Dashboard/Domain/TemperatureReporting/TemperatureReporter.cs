using Domain.Configuration;
using System.Data.SqlClient;
using Domain.Caching;

namespace Domain.TemperatureReporting
{
    public class TemperatureReporter
    {
        private const string _cacheKey = "TemperatureReportingService.Cache";
        private readonly ICacheService _cacheService;
        private string _connectionString;

        public TemperatureReporter(IConfigurationRetriever configurationRetriever, ICacheService cacheService)
        {
            _connectionString = configurationRetriever.Get("TemperatureReportingService.ConnectionString");
            _cacheService = cacheService;
        }

        public TemperatureReportAggregateResponse GetTemperatureRecent()
        {
            var cachedItem = _cacheService.Get<TemperatureReportAggregateResponse>(_cacheKey);
            if (cachedItem != null)
            {
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

                string sql =
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
            _cacheService.Set(_cacheKey, response, 60);
            Console.WriteLine($"{GetType().Name}: Read data from SQL source");

            return response;
        }
    }
}