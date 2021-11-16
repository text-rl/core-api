using System;
using System.Text;
using CoreApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace CoreApi.Infrastructure.Services.Messaging
{
    public abstract class RabbitMqBasePublisher
    {
        private readonly IModel? _channel;

        private readonly ILogger _logger;
        protected string? Exchange;

        protected string? RoutingKey;

        public RabbitMqBasePublisher(RabbitMqSettings settings, ILogger<RabbitMqBasePublisher> logger)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = settings.Host
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQClient init fail");
            }

            _logger = logger;
        }

        protected void PushMessage(object message)
        {
            _logger.LogInformation("PushMessage in {Exchange}, routing key:{RoutingKey}", Exchange, RoutingKey);
            _channel.ExchangeDeclare(Exchange, ExchangeType.Topic);

            string msgJson = JsonConvert.SerializeObject(message,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            var body = Encoding.UTF8.GetBytes(msgJson);
            _channel.BasicPublish(Exchange,
                RoutingKey,
                null,
                body);
        }
    }
}