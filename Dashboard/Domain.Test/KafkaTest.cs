using Domain.KafkaBroker;
using Xunit;

namespace Domain.Test
{
    public class KafkaTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("KafkaProvider.BootstrapServers", "172.17.179.38:9092");
            Environment.SetEnvironmentVariable("KafkaProvider.GroupId", "GroupForJll");
            var service = new KafkaBroker.KafkaBroker(null);
            await service.Produce("quickstart", "some testing stuff");
            service.StartConsumer("quickstart");
        }
    }
}
