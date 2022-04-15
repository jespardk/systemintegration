namespace Domain.PowerMeasurements
{
    public class PowerMeasurementCsvSanitizer
    {
        public static string Sanitize(string input)
        {
            input = input.Trim();

            var inputAsLines = input.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var headerIsReached = false;
            var sanitizedLines = new List<string>();

            for (int i = 0; i < inputAsLines.Length; i++)
            {
                string line = inputAsLines[i];
                bool isHeaderLine = line.StartsWith("INTERVAL;TIMESTAMP;SERIAL;P_AC;E_DAY");
                if (isHeaderLine)
                {
                    headerIsReached = true;
                }

                if (headerIsReached && !line.StartsWith("[wr"))
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
