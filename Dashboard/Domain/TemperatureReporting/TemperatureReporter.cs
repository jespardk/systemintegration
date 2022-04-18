using Domain.Configuration;
using System.Data.SqlClient;
using Domain.Caching;

namespace Domain.TemperatureReporting
{
    public class TemperatureReporter
    {
        private const string _cacheKey = "TemperatureReporting.Cache";
        private readonly ICacheService _cacheService;
        private string? _connectionString;

        public TemperatureReporter(IConfigurationRetriever configurationRetriever, ICacheService cacheService)
        {
            _connectionString = configurationRetriever.Get("TemperatureReporting.ConnectionString");
            _cacheService = cacheService;
        }

        public TemperatureReportAggregateResponse GetTemperatureRecent()
        {
            var response = new TemperatureReportAggregateResponse();
            response.DataSourceType = "SQL server";

            try
            {
                var cachedItem = _cacheService.Get<TemperatureReportAggregateResponse>(_cacheKey);
                if (cachedItem != null)
                {
                    cachedItem.IsFromCache = true;
                    return cachedItem;
                }

                response.DateTime = DateTime.Now;

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

                // Cache result
                _cacheService.Set(_cacheKey, response, 60);
                Console.WriteLine($"{GetType().Name}: Read data from SQL source");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error reading from resource. Message {sqlEx.Message}. Errors: {string.Join("; ", sqlEx.Errors)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from resource. Message {ex.Message}");
            }

            return response;
        }
    }
}