using Xunit;

namespace Case.Services.Test
{
    public class KafkaTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("KafkaProvider.BootstrapServers", "");
            Environment.SetEnvironmentVariable("KafkaProvider.GroupId", "");
            var service = new KafkaService(null);
            await service.Produce("quickstart", "some testing stuff");
            service.Consume("quickstart");
        }
    }
}
