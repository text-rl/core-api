using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Hosting;

namespace CoreApi.Web.Services
{
    internal class HeartbeatService : BackgroundService
    {
        #region Fields

        private const string HeartbeatMessageFormat = "Demo.AspNetCore.ServerSentEvents Heartbeat ({0} UTC)";

        private readonly IServerSentEventsService _serverSentEventsService;

        #endregion

        #region Constructor

        public HeartbeatService(IServerSentEventsService serverSentEventsService)
        {
            _serverSentEventsService = serverSentEventsService;
        }

        #endregion

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _serverSentEventsService.SendEventAsync(string.Format(HeartbeatMessageFormat, DateTime.UtcNow),
                    stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        #endregion
    }
}