namespace CoreApi.Infrastructure.Services.Messaging
{
    public interface IRabbitMqPublisher
    {
        void PushMessage(string queueName, object message);
    }
}