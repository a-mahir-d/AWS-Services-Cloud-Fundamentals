using SecretsManager.WebAPI.Options;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;
builder.Configuration.AddSecretsManager(configurator: options =>
{
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    options.KeyGenerator = (_, s) => s.Replace($"{env}_{appName}_", string.Empty).Replace("__", ":");
    options.PollingInterval = TimeSpan.FromSeconds(5);
});

builder.Services.AddControllers();

builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

app.MapControllers();

app.Run();