using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddAggregateRepositories();
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
    }
}