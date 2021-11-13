using System;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CoreApi.Infrastructure.Services.Messaging
{
    public abstract class BaseDispatcherService<TMessage> : RabbitMqBasePublisher, IDispatcher<TMessage>
    {
        protected BaseDispatcherService(RabbitMqSettings settings, ILogger<BaseDispatcherService<TMessage>> logger) :
            base(settings, logger)
        {
        }

        public void Dispatch(TMessage message)
        {
            PushMessage(message ??
                        throw new Exception("Cannot send null message with rabbitmq"));
        }
    }
}