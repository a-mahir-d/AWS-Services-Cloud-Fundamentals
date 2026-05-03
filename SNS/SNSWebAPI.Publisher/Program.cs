using SNSWebAPI.Publisher.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SendMessage>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
