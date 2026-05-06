using Amazon.S3;
using Customers.WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
