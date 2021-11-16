using System;
using System.Text;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Settings;
using CoreApi.Web.Messaging;
using CoreApi.Web.Services;
using CoreApi.Web.Sse;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CoreApi.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetRequiredSetting<JwtSettings>();
            services.AddSingleton(jwtSettings);
            if (jwtSettings.Key is null)
            {
                throw new Exception("JWt Key is null please check your JwtSettings");
            }

            if (jwtSettings.MinutesDuration is null)
            {
                throw new Exception("JWt MinutesDuration is null please check your JwtSettings");
            }

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
            return services;
        }

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddHostedService<DoneTreatmentListener>();
        }

        public static void AddMessageServices(this IServiceCollection services)
        {
            services.AddSingleton<ITextTreatmentMessageService, TextTreatmentMessageService>();
        }

        public static void AddSse(this IServiceCollection services)
        {
            services.AddServerSentEvents();
            services.AddSingleton<IHostedService, HeartbeatService>();
            services.AddSingleton<IServerSentEventsClientIdProvider, SseTokenClientIdProvider>();
            services.AddServerSentEvents<ISseService, SseService>();
        }
    }
}