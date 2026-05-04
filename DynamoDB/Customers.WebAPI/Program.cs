using Amazon.DynamoDBv2;
using Customers.WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
builder.Services.AddScoped<CustomerRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
