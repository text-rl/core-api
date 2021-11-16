using System.Linq;
using CoreApi.Infrastructure;
using CoreApi.Web.Extensions;
using CoreApi.Web.Sse;
using Lib.AspNetCore.ServerSentEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DependencyInjection = CoreApi.ApplicationCore.DependencyInjection;

const string CorsPolicyName = "AllowAll";


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(DependencyInjection.GetAssembly());
builder.Services.AddUserServices();
builder.Services.AddRabbitMq();
builder.Services.AddMessageServices();
builder.AddCors(CorsPolicyName);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreApi.Client", Version = "v1" });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddSse();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage()
        .UseSwagger()
        .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreApi.Client v1"));
}


app.UseRouting()
    .UseCors(CorsPolicyName)
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapServerSentEvents("/see-heartbeat");
        endpoints.MapServerSentEvents<SseService>("/sse-texttreatment");
        endpoints.MapControllers();
    });

app.Run();