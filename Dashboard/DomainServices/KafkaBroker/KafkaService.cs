using DomainServices.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace DomainServices.KafkaBroker
{
    public class KafkaService
    {
        private const string _cacheKey = "KafkaProvider.CacheKey";
        private ProducerConfig _producerConfig;
        private ConsumerConfig _consumerConfig;
        public delegate void NotifyOnNewMessage(); // delegate

        public KafkaService(IConfiguration? configuration)
        {
            var configService = new ConfigurationService(configuration);
            var servers = configService.GetConfigValue("KafkaProvider.BootstrapServers") ?? throw new ArgumentException("Config for KafkaProvider.BootstrapServers not found");
            var groupId = configService.GetConfigValue("KafkaProvider.GroupId") ?? throw new ArgumentException("Config for KafkaProvider.GroupId not found");

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

        public void Consume(string topic)
        {
            Console.WriteLine($"Starting consumer to listen...");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(topic);
                var totalCount = 0;
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"[Message receieved] {cr.Value}");
                        //totalCount += JObject.Parse(cr.Message.Value).Value<int>("count");
                        //Console.WriteLine($"Consumed record with key {cr.Message.Key} and value {cr.Message.Value}, and updated total count to {totalCount}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}