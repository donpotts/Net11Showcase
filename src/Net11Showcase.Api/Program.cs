using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net11Showcase.Data;
using Net11Showcase.Shared.Models;
using Net11Showcase.Shared.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Database & Repositories (100% Free SQLite local DB with Azure SQL option in appsettings)
var connString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=showcase_net11.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connString));

builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddSingleton<ShowcaseDataService>();

// 2. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// 3. OpenAPI 3.1 & Scalar Documentation UI
builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure DB is created & seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");

// Configure Scalar Interactive Documentation
app.MapOpenApi();
app.MapScalarApiReference(); // Accessible at /scalar/v1

// -------------------------------------------------------------
// Minimal API Endpoints (.NET 11 Showcase)
// -------------------------------------------------------------

// Health Probe
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "Healthy",
    framework = ".NET 11.0.0-rc.3",
    timestampUtc = DateTime.UtcNow,
    engine = "Net11Showcase.Api",
    scalarDocs = "/scalar/v1"
}))
.WithName("HealthCheck")
.WithSummary("Pre-flight health check endpoint for port coordination.");

// EF Core 11 Customers Endpoints
var customersGroup = app.MapGroup("/api/customers").WithTags("Customers (EF Core 11)");

customersGroup.MapGet("/", async (CustomerRepository repo) =>
{
    var list = await repo.GetAllAsync();
    return Results.Ok(list);
})
.WithSummary("Retrieves all enterprise customers with nested JSON metadata.");

customersGroup.MapGet("/top-tier", async (CustomerRepository repo) =>
{
    var top = await repo.GetHighestTierCustomerAsync();
    return top is not null ? Results.Ok(top) : Results.NotFound();
})
.WithSummary("Queries customer using modern LINQ MaxBy operator.");

// AI Vector Search Endpoint
app.MapGet("/api/ai/vector-search", async (string? query, CustomerRepository repo) =>
{
    var queryVec = new float[] { 0.25f, 0.75f, 0.50f, 0.80f };
    var results = await repo.SearchSimilarAsync(queryVec, topK: 3);
    return Results.Ok(new
    {
        query = query ?? ".NET 11 architecture",
        algorithm = "Cosine Similarity (In-Memory EF Core Vector Math)",
        results
    });
})
.WithTags("AI Services")
.WithSummary("Performs vector embedding cosine similarity search.");

// Benchmark Metrics Endpoint
app.MapGet("/api/benchmarks", (ShowcaseDataService data) =>
{
    return Results.Ok(data.GetFrameworkBenchmarks());
})
.WithTags("Benchmarks")
.WithSummary("Retrieves cross-version benchmarks (.NET 8 vs 9 vs 10 vs 11).");

// .NET 11 Response Streaming Endpoint (IAsyncEnumerable)
app.MapGet("/api/stream/telemetry", StreamTelemetry)
.WithTags("Streaming")
.WithSummary("Demonstrates .NET 11 Server-Sent IAsyncEnumerable response streaming.");

static async IAsyncEnumerable<SignalRTelemetryMetric> StreamTelemetry([EnumeratorCancellation] CancellationToken ct)
{
    for (int i = 0; i < 5; i++)
    {
        if (ct.IsCancellationRequested) yield break;

        yield return new SignalRTelemetryMetric
        {
            NodeId = $"node-stream-{i + 1}",
            ThroughputRps = 25000 + Random.Shared.Next(1000, 8000),
            LatencyP99Ms = 0.8 + (Random.Shared.NextDouble() * 0.5),
            ActiveConnections = 1200 + (i * 45),
            Timestamp = DateTime.UtcNow
        };

        await Task.Delay(250, ct);
    }
}

app.Run();
