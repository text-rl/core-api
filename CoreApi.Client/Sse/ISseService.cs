using System;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;

namespace CoreApi.Web.Sse
{
    public interface ISseService : IServerSentEventsService
    {
        Task SendAsync<T>(T data);
        Task SendToClientAsync<T>(T data, Guid clientId, string type);
        Task SendToClientAsync<T>(T data, Guid clientId);
    }
}