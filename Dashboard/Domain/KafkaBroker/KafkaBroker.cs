using Domain.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Domain.KafkaBroker
{
    public class KafkaBroker
    {
        private ProducerConfig _producerConfig;
        private ConsumerConfig _consumerConfig;
        private CancellationTokenSource _consumerCancellationToken;

        public event Action<string> MessageArrived;

        public KafkaBroker(IConfiguration? configuration)
        {
            var configurationRetriever = new ConfigurationRetriever(configuration);
            var servers = configurationRetriever.GetConfigValue("KafkaProvider.BootstrapServers");
            var groupId = configurationRetriever.GetConfigValue("KafkaProvider.GroupId");

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = servers,
                ClientId = Dns.GetHostName(),
                MessageTimeoutMs = 4000,
                SocketTimeoutMs = 4000
            };

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = servers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SocketTimeoutMs = 4000
            };
        }

        public async Task<bool> Produce(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                var produceResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

                var wasPersisted = produceResult.Status == PersistenceStatus.Persisted;
                Console.WriteLine("Persist OK for Kafka");

                return wasPersisted;
            }
        }

        public void BeginConsuming(string topic)
        {
            Console.WriteLine($"Starting consumer to listen...");
            _consumerCancellationToken = new CancellationTokenSource();

            using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(topic);

                Console.WriteLine($"Consumer ready");

                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(_consumerCancellationToken.Token);
                        Console.WriteLine($"[Message receieved] {cr.Value}");
                        MessageArrived(cr.Value);
                    }
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine($"Consumer stopped.");
                }
            }
        }

        public void StopConsuming()
        {
            _consumerCancellationToken.Cancel();
            Console.WriteLine($"Cancelling consumer...");
        }
    }
}