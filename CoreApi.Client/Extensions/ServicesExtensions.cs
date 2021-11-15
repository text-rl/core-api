using System;
using System.Text;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Settings;
using CoreApi.Web.Hubs;
using CoreApi.Web.Messaging;
using CoreApi.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreApi.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserIdProvider, UserIdProvider>();
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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments(TextTreatmentHub.Route))
                                // Read the token out of the query string
                                context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };
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
    }
}