using Domain.KafkaBroker;
using Domain.DanishEnergyPrices;

namespace Client.Middleware
{
    public static class StartSubscriptionsMiddleware
    {
        public static void StartSubscriptions(this IApplicationBuilder app)
        {
            Task task = Task.Run(() =>
            {
                var kafkaBroker = app.ApplicationServices.GetService<KafkaBroker>();

                Console.WriteLine("Starting subscription: " + nameof(IncomingDanishEnergyPriceHandler));
                var incomingPriceHandler = app.ApplicationServices.GetService<IncomingDanishEnergyPriceHandler>();
                kafkaBroker.MessageArrived += incomingPriceHandler.OnNewPricesReceived;
            });
        }
    }
}
