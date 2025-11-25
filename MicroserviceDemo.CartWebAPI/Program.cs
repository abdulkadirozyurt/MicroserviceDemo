using Consul;
using MicroserviceDemo.CartWebAPI.Context;
using MicroserviceDemo.CartWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("CartDb");
});

builder.Services.AddHttpClient();
builder.Services.AddConsulDiscoveryClient();

var app = builder.Build();

app.MapGet("carts",
    async (
        ApplicationDbContext context,
        IDiscoveryClient discoveryClient,
        HttpClient client,
        CancellationToken cancellationToken) =>
{
    var productServiceEndpoints = await discoveryClient.GetInstancesAsync("ProductWebAPI", cancellationToken);
    var productApiEndpoint = productServiceEndpoints.First().Uri;

    
    var carts = await context.Carts.ToListAsync();
    carts.Add(new Cart
    {
        ProductId = Guid.Parse("6a18b9d2-9537-4c12-86de-70bb61192ee0"),
        QuantityPerUnit = 1
    });

    var products = await client.GetFromJsonAsync<List<Product>>($"{productApiEndpoint}products");

    var cartsResult = carts.Select(s => new
    {
        Id = s.Id,
        ProductId = s.ProductId,
        Name = products!.FirstOrDefault(p => p.Id == s.ProductId)?.Name ?? string.Empty,
        QuantityPerUnit = s.QuantityPerUnit
    });

    return Results.Ok(cartsResult);
});

app.Run();
