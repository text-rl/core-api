using System;
using CoreApi.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CoreApi.Client.Messaging
{
    public abstract class NotifierListenerService<TCommand> : RabbitMqBaseListener where TCommand : INotification
    {
        private readonly IServiceProvider _serviceProvider;

        protected NotifierListenerService(RabbitMqSettings settings, ILogger<NotifierListenerService<TCommand>> logger,
            IServiceProvider serviceProvider) : base(settings, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public override bool Process(string message)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                Logger.LogInformation(
                    "RabbitListener message received on , exchange: {Exchange}, routeKey:{RoutingKey}",
                    Exchange, RoutingKey);
                var json = JObject.Parse(message);
                var runParticipation = JsonConvert.DeserializeObject<TCommand>(json.ToString(),
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                if (runParticipation is null)
                {
                    Logger.LogError("Cannot parse rabbitmq message");
                    return false;
                }

                publisher.Publish(runParticipation);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Process fail,error:{ExceptionMessage},stackTrace:{StackTrace},message:{Message}",
                    ex.Message, ex.StackTrace, message);
                Logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}