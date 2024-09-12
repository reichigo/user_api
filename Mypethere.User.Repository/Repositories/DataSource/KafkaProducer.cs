using Confluent.Kafka;

using MyPethere.User.CrossCutting.Kafka;
using System.Text.Json;

namespace Mypethere.User.Infrastructure.Repositories.DataSource;

public class KafkaProducer<T>(IProducer<Null, string> _producer) : IKafkaProducer<T>
{
    public async Task ProduceAsync(string topic, T message)
    {
        try
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedMessage });
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}
