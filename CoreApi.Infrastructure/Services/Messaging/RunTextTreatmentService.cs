using CoreApi.ApplicationCore.Dtos;
using CoreApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CoreApi.Infrastructure.Services.Messaging
{
    public class RunTextTreatmentService : BaseDispatcherService<RunTextTreatmentMessage>
    {
        public RunTextTreatmentService(RabbitMqSettings settings,
            ILogger<RunTextTreatmentService> logger) : base(settings, logger)
        {
            Exchange = settings.TreatmentExchange;
            RoutingKey = settings.PendingTreatmentRoutingKey;
        }
    }

  
}