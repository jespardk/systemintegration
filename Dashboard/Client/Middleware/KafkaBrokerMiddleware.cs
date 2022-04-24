using Domain.KafkaBroker;

namespace Client.Middleware
{
    public static class KafkaBrokerMiddleware
    {
        public static KafkaBroker? KafkabrokerInstance;

        public static void BootKafkaConsumer(this IApplicationBuilder app)
        {
            Task task = Task.Run(() =>
            {
                KafkabrokerInstance = app.ApplicationServices.GetService<KafkaBroker>();
                KafkabrokerInstance.StartConsumer("quickstart");
            });
        }
    }
}
