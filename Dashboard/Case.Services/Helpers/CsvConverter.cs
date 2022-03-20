using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Case.Services.Helpers
{
    public class CsvConverter
    {
        public static List<T>? Convert<T>(string input)
            where T : class, new()
        {
            try
            {
                var byteArray = Encoding.UTF8.GetBytes(input);
                var stream = new MemoryStream(byteArray);

                using StreamReader streamReader = new StreamReader(stream);

                var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    HasHeaderRecord = true,
                    Comment = '#',
                    AllowComments = true,
                    Delimiter = ";"
                };

                using var csvReader = new CsvReader(streamReader, csvConfig);

                var converted = csvReader.GetRecords<T>().ToList();

                return converted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
