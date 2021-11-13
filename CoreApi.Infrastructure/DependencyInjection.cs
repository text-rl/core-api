using System;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Dtos;
using CoreApi.Infrastructure.Database;
using CoreApi.Infrastructure.Extensions;
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
            return services.AddCommonServices().AddDatabase(configuration).AddRabbitMq(configuration);
        }

        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
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
            return serviceCollection.AddScoped<IDispatcher<RunTextTreatmentDto>, RunTextTreatmentService>();
        }
    }
}