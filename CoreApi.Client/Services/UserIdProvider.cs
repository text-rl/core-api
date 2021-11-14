using System;
using System.Security.Claims;
using CoreApi.ApplicationCore.Contracts;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApi.Web.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}