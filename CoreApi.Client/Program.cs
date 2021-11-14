using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Settings;
using CoreApi.Web;
using CoreApi.Web.Extensions;
using CoreApi.Web.Hubs;
using CoreApi.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "CoreApi.Client", Version = "v1" }); });
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreApi.Client v1"));
}


app.UseRouting();
app.UseCors(CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<TextTreatmentHub>(TextTreatmentHub.Route,
        options => options.Transports = HttpTransportType.ServerSentEvents);
});

app.Run();