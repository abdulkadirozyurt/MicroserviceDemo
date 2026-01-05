using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(cfg =>
{
    cfg.AddFixedWindowLimiter("fixed", x =>
    {
        x.QueueLimit = 2;
        x.PermitLimit = 5;
        x.Window = TimeSpan.FromSeconds(5);
        x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddOpenApi();


var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Microservice Demo API");
    options.AddDocument("Product API","", "/api/products/openapi/v1.json");
    options.AddDocument("Cart API", "", "/api/carts/openapi/v1.json");
});

app.MapReverseProxy();
app.UseRateLimiter();
app.MapGet("/", () => "Hello World!");


app.Run();
