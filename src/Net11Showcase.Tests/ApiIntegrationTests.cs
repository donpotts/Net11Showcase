using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Net11Showcase.Tests;

/// <summary>Hosts the real Net11Showcase.Api (real pipeline, real SQLite file) and calls it over HTTP.</summary>
public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"net11showcase-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", $"Data Source={_dbPath}");
        builder.UseSetting("RateLimiting:AiPermitLimit", "3");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        try { File.Delete(_dbPath); } catch (IOException) { /* best effort */ }
    }
}

public class ApiIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public ApiIntegrationTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Health_RunsTheRegisteredChecks_IncludingTheDatabase()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Customers_ComeBackWithTheirJsonProfile()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/customers"));

        var first = doc.RootElement.EnumerateArray().First(c => c.GetProperty("id").GetInt32() == 1);
        Assert.Equal("FinTech", first.GetProperty("profile").GetProperty("industry").GetString());
    }

    [Fact]
    public async Task VectorSearch_UsesTheQuery_NotAHardcodedVector()
    {
        var client = _factory.CreateClient();

        // Different queries embed to different vectors, so the similarity scores differ.
        var a = await client.GetStringAsync("/api/ai/vector-search?query=native%20aot%20startup");
        var b = await client.GetStringAsync("/api/ai/vector-search?query=json%20columns%20in%20sqlite");

        using var da = JsonDocument.Parse(a);
        using var db = JsonDocument.Parse(b);
        Assert.Equal("native aot startup", da.RootElement.GetProperty("query").GetString());
        Assert.NotEqual(
            da.RootElement.GetProperty("results").EnumerateArray().Select(r => r.GetProperty("cosineSimilarity").GetDouble()).ToArray(),
            db.RootElement.GetProperty("results").EnumerateArray().Select(r => r.GetProperty("cosineSimilarity").GetDouble()).ToArray());
    }

    [Fact]
    public async Task ServerSentEvents_StreamTelemetryFrames_AsEventStream()
    {
        var client = _factory.CreateClient();

        using var response = await client.GetAsync("/api/stream/telemetry/sse", HttpCompletionOption.ResponseHeadersRead);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);
        var frames = body.Split('\n').Where(l => l.StartsWith("event: telemetry")).ToList();
        Assert.Equal(5, frames.Count);
        var firstData = body.Split('\n').First(l => l.StartsWith("data: ")).Substring("data: ".Length);
        using var payload = JsonDocument.Parse(firstData);
        Assert.StartsWith("node-stream-", payload.RootElement.GetProperty("nodeId").GetString());
    }
}

/// <summary>Own fixture so the rate limiter's window is not shared with the other tests' vector-search calls.</summary>
public class RateLimitingTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public RateLimitingTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task AiEndpoint_IsLimitedPerClient_AndAnswers429WhenExceeded()
    {
        var client = _factory.CreateClient();

        var statuses = new System.Collections.Generic.List<HttpStatusCode>();
        for (var i = 0; i < 5; i++)
            statuses.Add((await client.GetAsync("/api/ai/vector-search?query=limit")).StatusCode);

        Assert.Equal(3, statuses.Count(s => s == HttpStatusCode.OK));          // configured permit limit
        Assert.Equal(2, statuses.Count(s => s == HttpStatusCode.TooManyRequests));
        // Other endpoints are not affected by the AI policy.
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/benchmarks")).StatusCode);
    }
}
