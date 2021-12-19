using System;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreApi.Client.Sse
{
    public class SseService : ServerSentEventsService, ISseService
    {
        private record Event<TData>(TData Data, string? Type);

        public SseService(IOptions<ServerSentEventsServiceOptions<SseService>> options)
            : base(options.ToBaseServerSentEventsServiceOptions())
        {
        }

        public Task SendAsync<T>(T data)
        {
            return SendEventAsync(JsonConvert.SerializeObject(data));
        }

        public Task SendToClientAsync<T>(T data, Guid clientId, string type)
        {
            var e = new Event<T>(data, type);
            return GetClient(clientId).SendEventAsync(JsonConvert.SerializeObject(e,
                new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        public Task SendToClientAsync<T>(T data, Guid clientId)
        {
            var e = new Event<T>(data, default);
            return GetClient(clientId).SendEventAsync(JsonConvert.SerializeObject(e,
                new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}