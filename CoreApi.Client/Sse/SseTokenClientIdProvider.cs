using System;
using System.Security.Claims;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Http;

namespace CoreApi.Client.Sse
{
    public class SseTokenClientIdProvider : IServerSentEventsClientIdProvider
    {
        public Guid AcquireClientId(HttpContext context)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.Name);
            return userId is null ? Guid.NewGuid() : Guid.Parse(userId);
        }

        public void ReleaseClientId(Guid clientId, HttpContext context)
        {
        }
    }
}