using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Case.Services.Helpers
{
    public class CsvHandler
    {
        public static List<T> ConvertCsvV2<T>(string input)
            where T : class, new()
        {
            try
            {
                var sanitizedInput = SanitizeInput(input);

                var byteArray = Encoding.UTF8.GetBytes(sanitizedInput);
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

        private static string SanitizeInput(string input)
        {
            input = input.Trim();

            var inputAsLines = input.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var headerFound = false;
            var sanitizedLines = new List<string>();

            for (int i = 0; i < inputAsLines.Length; i++)
            {
                string line = inputAsLines[i];
                bool isHeaderLine = line.StartsWith("INTERVAL;TIMESTAMP;SERIAL;P_AC;E_DAY");

                if (isHeaderLine)
                {
                    headerFound = true;
                }

                if (headerFound && !line.StartsWith("[wr"))
                {
                    sanitizedLines.Add(line);
                }
                else
                {
                    continue;
                }
            }

            return string.Join("\r\n", sanitizedLines);
        }
    }
}
