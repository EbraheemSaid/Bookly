using Bookly.Api.Extensions;
using Bookly.Application;
using Bookly.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Map OpenAPI and Scalar in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Bookly API");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    // Redirect root to Scalar API reference
    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
       .ExcludeFromDescription();

    app.ApplyMigrations();
    //app.SeedData();
}

app.UseCustomExceptionHandler();
//app.UseAuthorization();
app.MapControllers();

app.Run();
