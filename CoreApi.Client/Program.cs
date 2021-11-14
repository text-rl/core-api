using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure;
using CoreApi.Web.Hubs;
using CoreApi.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjection = CoreApi.ApplicationCore.DependencyInjection;

void AddServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    webApplicationBuilder.Services.AddTransient<IUserIdProvider, UserIdProvider>();
    webApplicationBuilder.Services.AddSingleton<ITextTreatmentMessageService, TextTreatmentHub>();
}


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(DependencyInjection.GetAssembly());
AddServices(builder);
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<TextTreatmentHub>(TextTreatmentHub.Route,
        options => options.Transports = HttpTransportType.ServerSentEvents);
});

app.Run();