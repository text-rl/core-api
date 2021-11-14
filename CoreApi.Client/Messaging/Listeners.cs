using System;
using CoreApi.ApplicationCore.Write.TextTreatments;
using CoreApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CoreApi.Web.Messaging
{
    public class DoneTreatmentListener : NotifierListenerService<TreatmentDoneNotification>
    {
        public DoneTreatmentListener(RabbitMqSettings settings,
            ILogger<DoneTreatmentListener> logger, IServiceProvider serviceProvider) : base(settings, logger,
            serviceProvider)
        {
            Exchange = settings.TreatmentExchange;
            RoutingKey = settings.DoneTreatmentRoutingKey;
        }
    }

}