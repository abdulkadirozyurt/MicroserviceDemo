using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(cfg =>
{
    cfg.AddFixedWindowLimiter("fixed", x =>
    {
        x.QueueLimit = 1;
        x.PermitLimit = 1;
        x.Window = TimeSpan.FromSeconds(60);
        x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

var app = builder.Build();

app.MapReverseProxy();
app.UseRateLimiter();

app.MapGet("/", () => "Hello World!");

app.Run();
