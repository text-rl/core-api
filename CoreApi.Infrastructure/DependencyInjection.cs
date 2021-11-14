﻿using System;
using System.Text;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Dtos;
using CoreApi.ApplicationCore.Read.Contracts;
using CoreApi.ApplicationCore.Write.Contracts;
using CoreApi.Infrastructure.Database;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Repository.AggregateRepositories;
using CoreApi.Infrastructure.Repository.ReadRepositories;
using CoreApi.Infrastructure.Services;
using CoreApi.Infrastructure.Services.Messaging;
using CoreApi.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCommonServices()
                .AddDatabase(configuration)
                .AddRabbitMq(configuration)
                .AddReadRepositories()
                .AddAggregateRepositories()
                .AddJwt(configuration);
        }

        public static IServiceCollection AddReadRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IReadUserRepository, ReadUserRepository>();
        }

        public static IServiceCollection AddAggregateRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }

        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ITokenService, TokenService>();
            return services.AddSingleton<ITimeService, TimeService>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetRequiredSetting<DatabaseSettings>();
            if (dbSettings.ConnectionString is null)
            {
                throw new Exception("Please provide connection string");
            }

            return services.AddDbContext<CoreApiContext>(options =>
                options.UseSqlServer(dbSettings.ConnectionString,
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            );
        }
        


        public static IServiceCollection AddRabbitMq(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            // RabbitMQ
            var rabbitMqSettings = configuration.GetRequiredSetting<RabbitMqSettings>();
            serviceCollection.AddSingleton(rabbitMqSettings);
            return serviceCollection.AddScoped<IDispatcher<RunTextTreatmentMessage>, RunTextTreatmentService>();
        }

        private static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
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
    }
}