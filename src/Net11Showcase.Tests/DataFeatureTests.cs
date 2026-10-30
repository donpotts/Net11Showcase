using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Net11Showcase.Data;
using Xunit;

namespace Net11Showcase.Tests;

/// <summary>
/// Runs against real SQLite (not the EF in-memory provider, which cannot translate JSON columns) so these
/// tests prove the SQL that EF Core 11 actually generates and executes.
/// </summary>
public sealed class JsonColumnTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly AppDbContext _db;

    public JsonColumnTests()
    {
        _connection.Open();
        _db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);
        DatabaseInitializer.Initialize(_db);
    }

    [Fact]
    public async Task Profile_IsStoredAsOneJsonDocument_InTheCustomerRow()
    {
        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Profile FROM Customers WHERE Id = 1";
        var json = (string?)await cmd.ExecuteScalarAsync();

        Assert.NotNull(json);
        using var doc = JsonDocument.Parse(json!);
        Assert.Equal("FinTech", doc.RootElement.GetProperty("Industry").GetString());
        Assert.Equal("US-East", doc.RootElement.GetProperty("Region").GetString());
        Assert.Equal(450, doc.RootElement.GetProperty("Seats").GetInt32());
    }

    [Fact]
    public async Task FilterOnJsonMember_ReturnsTheMatchingCustomer()
    {
        var repo = new CustomerRepository(_db);

        var result = await repo.GetByRegionAsync("EU-North");

        var customer = Assert.Single(result);
        Assert.Equal("Elena Rostova", customer.FullName);
        Assert.Equal("BioTech", customer.Profile!.Industry);
    }

    [Fact]
    public void FilterOnJsonMember_IsTranslatedToSql_NotEvaluatedInMemory()
    {
        var repo = new CustomerRepository(_db);

        var sql = repo.RegionQuery("US-East").ToQueryString();

        // EF emits SQLite JSON access over the Profile column (->> or json_extract), not a table scan + client filter.
        Assert.Contains("Profile", sql);
        Assert.True(sql.Contains("->>") || sql.Contains("json_extract", StringComparison.OrdinalIgnoreCase), sql);
        Assert.Contains("WHERE", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GroupByJsonMember_CountsInTheDatabase()
    {
        var breakdown = await new CustomerRepository(_db).GetIndustryBreakdownAsync();

        Assert.Equal(3, breakdown.Count);
        Assert.Equal(1, breakdown["FinTech"]);
        Assert.Equal(1, breakdown["Cybersecurity"]);
        Assert.Equal(1, breakdown["BioTech"]);
    }

    [Fact]
    public async Task UpdatingAJsonMember_RoundTrips()
    {
        var sarah = await _db.Customers.SingleAsync(c => c.Id == 1);
        sarah.Profile!.Seats = 900;
        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();

        var reloaded = await _db.Customers.AsNoTracking().SingleAsync(c => c.Id == 1);
        Assert.Equal(900, reloaded.Profile!.Seats);
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        await _connection.DisposeAsync();
    }
}

public class HashingEmbedderTests
{
    [Fact]
    public void SameText_AlwaysProducesTheSameVector()
    {
        Assert.Equal(HashingEmbedder.Embed("Native AOT startup"), HashingEmbedder.Embed("Native AOT startup"));
    }

    [Fact]
    public void CaseAndPunctuation_DoNotChangeTheVector()
    {
        Assert.Equal(HashingEmbedder.Embed("EF Core vector search"), HashingEmbedder.Embed("ef core, vector SEARCH!"));
    }

    [Fact]
    public void DifferentText_ProducesDifferentVectors()
    {
        Assert.NotEqual(HashingEmbedder.Embed("blazor rendering modes"), HashingEmbedder.Embed("sqlite json columns benchmark"));
    }

    [Fact]
    public void Vector_IsUnitLength_AndHasTheRequestedDimensions()
    {
        var v = HashingEmbedder.Embed("the quick brown fox", dimensions: 16);

        Assert.Equal(16, v.Length);
        Assert.Equal(1.0, Math.Sqrt(v.Sum(x => (double)x * x)), precision: 5);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EmptyText_ProducesAZeroVector(string? text)
    {
        Assert.All(HashingEmbedder.Embed(text), x => Assert.Equal(0f, x));
    }

    [Fact]
    public async Task QueryDrivesRanking_SoTheSameCustomerTopsTheirOwnEmbedding()
    {
        using var conn = new SqliteConnection("Data Source=:memory:");
        conn.Open();
        await using var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(conn).Options);
        DatabaseInitializer.Initialize(db);
        var repo = new CustomerRepository(db);

        // Searching with exactly Marcus's stored embedding must rank Marcus first with similarity ~1.
        var marcus = await db.Customers.AsNoTracking().SingleAsync(c => c.Id == 2);
        var top = (await repo.SearchSimilarAsync(marcus.EmbeddingVector, topK: 3)).First();

        Assert.Equal("Marcus Vance", top.Title);
        Assert.Equal(1.0, top.CosineSimilarity, precision: 3);
    }
}
