using System;
using System.Collections.Generic;
using System.Linq;
using Net11Showcase.Shared.Models;

namespace Net11Showcase.Shared.Services;

public sealed class ShowcaseDataService
{
    public List<QuickGridEnhancementItem> GetQuickGridItems() => new()
    {
        new() { Id = 101, Name = "Telemetry Stream Processor", Category = "Compute", Department = "Cloud Core", Score = 98.4, Status = "Optimal", LastActivityUtc = DateTime.UtcNow.AddMinutes(-2) },
        new() { Id = 102, Name = "PQC ML-KEM Cryptographic Enclave", Category = "Security", Department = "Zero-Trust SecOps", Score = 99.8, Status = "Enforced", LastActivityUtc = DateTime.UtcNow.AddMinutes(-5) },
        new() { Id = 103, Name = "Zstandard High-Throughput Compressor", Category = "Transport", Department = "Data Engineering", Score = 94.2, Status = "Optimal", LastActivityUtc = DateTime.UtcNow.AddMinutes(-8) },
        new() { Id = 104, Name = "SignalR Real-Time Message Bus", Category = "WebSockets", Department = "Full-Stack Blazor", Score = 97.6, Status = "Active", LastActivityUtc = DateTime.UtcNow.AddMinutes(-12) },
        new() { Id = 105, Name = "EF Core 11 Vector Embeddings Matcher", Category = "Database", Department = "AI & Search", Score = 96.1, Status = "Active", LastActivityUtc = DateTime.UtcNow.AddMinutes(-15) },
        new() { Id = 106, Name = "Native AOT CLI Diagnostic Tool", Category = "Tooling", Department = "DevOps Velocity", Score = 99.1, Status = "Compiled", LastActivityUtc = DateTime.UtcNow.AddMinutes(-22) },
        new() { Id = 107, Name = "MAUI HybridWebView Sensor Gateway", Category = "Mobile", Department = "Cross-Platform", Score = 95.8, Status = "Connected", LastActivityUtc = DateTime.UtcNow.AddMinutes(-28) }
    };

    public List<BenchmarkComparisonResult> GetFrameworkBenchmarks() => new()
    {
        new() { FrameworkVersion = ".NET 8", RuntimeMode = "Standard JIT", StartupTimeMs = 210.4, ThroughputKiloRps = 142.5, MemoryWorkingSetMb = 64.2, PublishSizeMb = 82.0, WasmLoadTimeMs = 820.0, EfficiencyRating = "B" },
        new() { FrameworkVersion = ".NET 9", RuntimeMode = "Tiered PGO JIT", StartupTimeMs = 145.2, ThroughputKiloRps = 198.4, MemoryWorkingSetMb = 48.5, PublishSizeMb = 65.4, WasmLoadTimeMs = 540.0, EfficiencyRating = "A" },
        new() { FrameworkVersion = ".NET 10", RuntimeMode = "Optimized JIT", StartupTimeMs = 112.8, ThroughputKiloRps = 245.0, MemoryWorkingSetMb = 39.2, PublishSizeMb = 52.8, WasmLoadTimeMs = 380.0, EfficiencyRating = "A" },
        new() { FrameworkVersion = ".NET 11", RuntimeMode = "Dynamic AOT + JIT", StartupTimeMs = 58.4, ThroughputKiloRps = 328.6, MemoryWorkingSetMb = 26.4, PublishSizeMb = 34.2, WasmLoadTimeMs = 210.0, EfficiencyRating = "A+ (Optimal)" },
        new() { FrameworkVersion = ".NET 11", RuntimeMode = "Native AOT (No JIT)", StartupTimeMs = 8.2, ThroughputKiloRps = 415.0, MemoryWorkingSetMb = 14.8, PublishSizeMb = 18.5, WasmLoadTimeMs = 95.0, EfficiencyRating = "S (Maximum Velocity)" }
    };

    public List<VectorSearchResult> PerformVectorSearch(string query, int topK = 3)
    {
        var sampleCorpus = new List<VectorSearchResult>
        {
            new() { EntityId = 1, Title = ".NET 11 Dynamic Native AOT & Tiered Compilation", Content = "Enables sub-10ms process boot times with zero runtime warm-up latency and small binary footprints.", CosineSimilarity = 0.962, MatchedCategory = "Runtime & AOT" },
            new() { EntityId = 2, Title = "Blazor Streaming & Relative Routing Engine", Content = "Asynchronously renders heavy layout components while streaming initial server HTML shells instantly.", CosineSimilarity = 0.914, MatchedCategory = "Frontend Blazor" },
            new() { EntityId = 3, Title = "EF Core 11 JSON Column Aggregates & Vector Search", Content = "Deep querying over nested JSON properties and cosine similarity nearest-neighbor vector indexes.", CosineSimilarity = 0.887, MatchedCategory = "Data & Storage" },
            new() { EntityId = 4, Title = "MAUI Blazor Hybrid CoreCLR Hardware Interop", Content = "Native device sensor bridges covering GPS, Camera, Gyroscope, and local offline SQLite background synchronization.", CosineSimilarity = 0.842, MatchedCategory = "Mobile Hybrid" }
        };

        return sampleCorpus.Take(topK).ToList();
    }
}
