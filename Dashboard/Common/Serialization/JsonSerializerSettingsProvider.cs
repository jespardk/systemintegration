using Newtonsoft.Json;

namespace Common.Serialization
{
    public class JsonSerializerSettingsProvider
    {
        public static JsonSerializerSettings WithSilentErrorHandling()
        {
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; }
            };

            return settings;
        }
    }
}
