using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Case.Services.Helpers
{
    public class CsvHandler
    {
        public static IList<T> ConvertCsv<T>(StreamReader reader)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Comment = '#',
                AllowComments = true,
                Delimiter = ","
            };

            //string value = reader.ReadToEnd();
            using (var csvReader = new CsvReader(reader, csvConfig))
            {
                var response = csvReader.GetRecords<T>();
                return response.ToList();
            }
        }
    }
}
