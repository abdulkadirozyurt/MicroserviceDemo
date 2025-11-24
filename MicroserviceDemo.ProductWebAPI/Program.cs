using MicroserviceDemo.ProductWebAPI.Context;
using MicroserviceDemo.ProductWebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("ProductDb");
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet(string.Empty, () => "Welcome to the Product Web API!");

app.MapGet("products", async (ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    var products = await context.Products.ToListAsync(cancellationToken);
    products.Add(new Product
    {
        Id=Guid.Parse("6a18b9d2-9537-4c12-86de-70bb61192ee0"),
        Name = "Smartphone",
        StockAmount = 50,
    });

    return Results.Ok(products);
});

app.MapHealthChecks("/health");

app.Run();
