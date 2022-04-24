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
        private IConsumer<string, string> _consumer;

        public event Action<string> MessageArrived;

        public KafkaBroker(IConfigurationRetriever configurationRetriever)
        {
            var servers = configurationRetriever.Get("KafkaProvider.BootstrapServers");
            var groupId = configurationRetriever.Get("KafkaProvider.GroupId");

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
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    var produceResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

                    var wasPersisted = produceResult.Status == PersistenceStatus.Persisted;
                    Console.WriteLine("Persist OK for Kafka");

                    return wasPersisted;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kafka error: " + ex.Message);
                return false;
            }
        }

        public void StartConsumer(string topic)
        {
            try
            {
                Console.WriteLine($"Starting consumer to listen...");
                _consumerCancellationToken = new CancellationTokenSource();

                _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
                _consumer.Subscribe(topic);

                Console.WriteLine($"Consumer ready");

                while (true)
                {
                    var cr = _consumer.Consume(_consumerCancellationToken.Token);

                    var lastIndex = cr.Value.Length > 30 ? 30 : cr.Value.Length;

                    Console.WriteLine($"[Message receieved] {cr.Value.Substring(0, lastIndex)}" + (cr.Value.Length >= 30 ? " (Truncated)" : ""));

                    MessageArrived(cr.Value);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _consumer.Close();
                Console.WriteLine($"Consumer stopped.");
            }
        }

        public void StopConsuming()
        {
            _consumer.Close();
            _consumerCancellationToken.Cancel();
            Console.WriteLine($"Cancelling consumer...");
        }
    }
}