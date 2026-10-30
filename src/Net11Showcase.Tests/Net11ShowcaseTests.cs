using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Net11Showcase.Data;
using Net11Showcase.Shared.Models;
using Net11Showcase.Shared.Services;
using Xunit;

namespace Net11Showcase.Tests;

public class Net11ShowcaseTests
{
    [Fact]
    public void ShowcaseDataService_Returns_All_Six_Modules_Data()
    {
        var service = new ShowcaseDataService();

        // 1. QuickGrid items
        var gridItems = service.GetQuickGridItems();
        Assert.NotNull(gridItems);
        Assert.NotEmpty(gridItems);

        // 2. Multi-version benchmarks (.NET 8, 9, 10, 11)
        var benchmarks = service.GetFrameworkBenchmarks();
        Assert.NotNull(benchmarks);
        Assert.Equal(5, benchmarks.Count);

        var net11Aot = benchmarks.FirstOrDefault(b => b.FrameworkVersion == ".NET 11" && b.RuntimeMode.Contains("Native AOT"));
        Assert.NotNull(net11Aot);
        Assert.True(net11Aot.StartupTimeMs < 10.0);
        Assert.True(net11Aot.ThroughputKiloRps > 400.0);

        // 3. Vector search
        var vectorMatches = service.PerformVectorSearch("Zero-Trust", topK: 2);
        Assert.Equal(2, vectorMatches.Count);
        Assert.True(vectorMatches[0].CosineSimilarity > 0.8);
    }

    [Fact]
    public async Task CustomerRepository_Queries_Json_And_Vectors()
    {
        // Real SQLite instead of the EF in-memory provider: the model maps Profile as a JSON column, which the
        // in-memory provider cannot materialize. EnsureCreated seeds three demo rows; this test wants only its own.
        using var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

        using var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        await db.Customers.ExecuteDeleteAsync();
        db.Customers.AddRange(
            new CustomerEntity
            {
                Id = 1,
                FullName = "Alice Walker",
                Email = "alice@cloud.io",
                Tier = "Tier 1",
                MetadataJson = "{\"industry\":\"HealthCare\"}",
                EmbeddingVector = new float[] { 0.2f, 0.4f, 0.6f },
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new CustomerEntity
            {
                Id = 2,
                FullName = "Bob Vance",
                Email = "bob@refrigeration.com",
                Tier = "Tier 2",
                MetadataJson = "{\"industry\":\"Logistics\"}",
                EmbeddingVector = new float[] { 0.8f, 0.6f, 0.4f },
                CreatedAt = DateTime.UtcNow
            }
        );
        await db.SaveChangesAsync();

        var repo = new CustomerRepository(db);

        // Test GetAllAsync
        var all = await repo.GetAllAsync();
        Assert.Equal(2, all.Count);

        // Test Modern LINQ MaxBy
        var latest = await repo.GetHighestTierCustomerAsync();
        Assert.NotNull(latest);
        Assert.Equal("Bob Vance", latest.FullName);

        // Test Vector Similarity Search
        var searchHits = await repo.SearchSimilarAsync(new float[] { 0.8f, 0.6f, 0.4f }, topK: 1);
        Assert.Single(searchHits);
        Assert.Equal("Bob Vance", searchHits[0].Title);
        Assert.True(searchHits[0].CosineSimilarity > 0.95);
    }
}
