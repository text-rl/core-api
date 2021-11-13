using System.Reflection;
using CoreApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjection = CoreApi.ApplicationCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMediatR(DependencyInjection.GetAssembly());
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "CoreApi.Web", Version = "v1" }); });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreApi.Web v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();