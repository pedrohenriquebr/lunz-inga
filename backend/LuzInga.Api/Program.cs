using Microsoft.OpenApi.Models;
using LuzInga.Infra;
using LuzInga.Application;
using LuzInga.Domain;
using Microsoft.AspNetCore.Mvc;
using LuzInga.Domain.Entities;
using LuzInga.Api;
using System.Text;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LuzIngaServer", Version = "v1" });
    c.EnableAnnotations();
});


var isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is not null;
var appsettingsFile = new StringBuilder()
    .Append("appsettings.")
    .Append(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
    .Append(".")
    .Append(isRunningInContainer switch {
        true => "container.json",
        _ => "json"
    })
    .ToString();

builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(
        appsettingsFile, 
        optional: true, 
        reloadOnChange: true
    );


builder
    .AddStartupHandler()
    .AddInfra()
    .AddDomain()
    .AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.UseResponseCaching();
app.UseResponseCachingExtended();
app.UseHttpLogging();
app.UseLogging();
app.UseSession();
app.MapControllers();
app.MapHangfireDashboard();
app.Run();
