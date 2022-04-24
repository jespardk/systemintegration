using Newtonsoft.Json;

namespace Common.Serialization
{
    public class JsonSerializerSettingsProvider
    {
        public static Newtonsoft.Json.JsonSerializerSettings WithSilentErrorHandling()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; }
            };

            return settings;
        }
    }
}
