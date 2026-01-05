using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                    .AddJsonFile("ocelot.json");
builder.Services.AddOcelot(builder.Configuration).AddPolly();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x
.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()
.SetPreflightMaxAge(TimeSpan.FromMinutes(10))           // Cache the preflight (OPTIONS request for CORS check from same origin) response for 10 minutes
);


app.MapGet("/", () => "Hello World!");


await app.UseOcelot();

await app.RunAsync();
