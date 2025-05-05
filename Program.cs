using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;
using mk_hangfire;

// Create the builder and the app
var builder = WebApplication.CreateBuilder(args);

// Get app settings path
var appSettingsPath = Environment.GetEnvironmentVariable("APPSETTINGS_PATH");

if (string.IsNullOrEmpty(appSettingsPath))
    appSettingsPath = "./appsettings.json";

// Load the app settings
var settings = new AppSettings();
var configuration = new ConfigurationBuilder()
    .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true) //load local settings
    .Build();
    
configuration.Bind(settings);

// Register settings in DI
builder.Services.Configure<AppSettings>(configuration);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

// Get connection string
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connString);
    });
    config.UseConsole();
});

// Set job expiration timeout
GlobalConfiguration
    .Configuration.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connString);
    })
    .WithJobExpirationTimeout(TimeSpan.FromDays(7));

builder.Services.AddHangfireServer();

// Construct app
var app = builder.Build();

// Log appsettings path
app.Logger.LogInformation($"Using appsettings.json file at: {appSettingsPath}");

// Allow hangfire dashboard access with authorization
var options = new DashboardOptions { Authorization = [new HangfireAuthorizationFilter()] };
app.UseHangfireDashboard("", options);

// Configure recurring job
string[] argument = ["non-bundle"];
RecurringJob.AddOrUpdate<item_synchronizer.SynchronizerRunner>(
    recurringJobId: "update-product-dimensions",
    methodCall: job => job.Run(argument, null),
    cronExpression: Cron.Never
);

app.Run("http://0.0.0.0:5789");
