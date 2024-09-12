namespace MyPethere.User.CrossCutting.Kafka;

public interface IKafkaProducer<T>
{
    Task ProduceAsync(string topic, T message);
}
