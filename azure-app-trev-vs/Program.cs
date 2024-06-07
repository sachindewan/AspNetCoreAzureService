using azure_app_trev_vs.Data;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using azure_app_trev_vs.Middleware;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AzureDataBase");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// Add services to the container.
builder.Services.AddRazorPages();
//Log.Logger = new LoggerConfiguration()
//         .MinimumLevel.Information()
//         .Enrich.FromLogContext()
//         .WriteTo.Console()
//         .WriteTo.ApplicationInsights(new TelemetryConfiguration
//         {
//             InstrumentationKey = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
//         }, TelemetryConverter.Traces)
//         .CreateLogger();

//builder.Host.UseSerilog();
//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_ConnectionString"]);
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
// Register middleware to handle correlation IDs
builder.Services.AddTransient<CorrelationIdMiddleware>();
Log.Information("Starting web host");
// Add Serilog request logging
//builder.Host.UseSerilog((context, services, configuration) => configuration
//    .ReadFrom.Configuration(context.Configuration)
//    .ReadFrom.Services(services)
//    .Enrich.FromLogContext()
//    .WriteTo.Console()
//    .WriteTo.ApplicationInsights(new TelemetryConfiguration { ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] }, TelemetryConverter.Traces));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext().Enrich.WithCorrelationId());
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();
app.MapRazorPages();

app.Run();
