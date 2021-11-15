using System;
using System.Security.Claims;
using CoreApi.ApplicationCore.Contracts;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreApi.Web.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var id = connection.User.FindFirstValue(ClaimTypes.Name);
            return id;
        }
    }
}