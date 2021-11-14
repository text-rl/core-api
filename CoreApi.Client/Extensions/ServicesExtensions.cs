using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Settings;
using CoreApi.Web.Hubs;
using CoreApi.Web.Messaging;
using CoreApi.Web.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApi.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserIdProvider, UserIdProvider>();
        }

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddHostedService<DoneTreatmentListener>();
        }

        public static void AddMessageServices(this IServiceCollection services)
        {
            services.AddSingleton<ITextTreatmentMessageService, TextTreatmentHub>();
        }

     
    }
}