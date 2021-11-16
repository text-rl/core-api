using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApi.Web.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddCors(this WebApplicationBuilder builder, string corsPolicyName)
        {
            var corsSettings = builder.Configuration.GetRequiredSetting<CorsSettings>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, b =>
                {
                    b.WithOrigins(corsSettings.FrontEndUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Location");
                });
            });
        }
    }
}