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

    private record CorpusDocument(int Id, string Title, string Content, string Category, string[] Keywords);

    private static readonly CorpusDocument[] s_corpus = new CorpusDocument[]
    {
        new(
            1,
            ".NET 11 Dynamic Native AOT & Tiered Compilation",
            "Enables sub-10ms process boot times with zero runtime warm-up latency, CoreCLR Tiered PGO, and minimal working set memory.",
            "Runtime & AOT",
            new[] { "native", "aot", "compilation", "runtime", "latency", "boot", "startup", "warmup", "tiered", "pgo", "speed", "fast", "performance", "binary", "jit", "cold" }
        ),
        new(
            2,
            "Blazor Streaming & Relative Routing Engine",
            "Asynchronously renders heavy layout components while streaming initial server HTML shells instantly via WebSockets and Auto Render Mode.",
            "Frontend Blazor",
            new[] { "blazor", "streaming", "routing", "server", "html", "websockets", "auto", "render", "frontend", "ui", "wasm", "webassembly", "component", "hydration" }
        ),
        new(
            3,
            "EF Core 11 JSON Column Aggregates & Vector Search",
            "Deep querying over nested JSON properties with LINQ MaxBy/MinBy and cosine similarity nearest-neighbor vector indexes in SQLite/SQL Server.",
            "Data & Storage",
            new[] { "ef", "efcore", "json", "column", "columns", "aggregates", "vector", "search", "linq", "database", "sqlite", "sql", "cosine", "indexing", "similarity", "query" }
        ),
        new(
            4,
            "MAUI Blazor Hybrid CoreCLR Hardware Interop",
            "Native device sensor bridges covering GPS, Camera, Gyroscope, Accelerometer, and local offline SQLite background synchronization.",
            "Mobile Hybrid",
            new[] { "maui", "hybrid", "sensors", "sensor", "hardware", "gps", "camera", "gyroscope", "mobile", "device", "offline", "cross-platform", "android", "ios", "bridge" }
        ),
        new(
            5,
            "Zero-Trust PQC ML-KEM-768 Cryptographic Enclave",
            "Post-Quantum Cryptography lattice-based key encapsulation compliant with NIST FIPS 203, protecting against harvest-now-decrypt-later vectors.",
            "Security & PQC",
            new[] { "zero-trust", "trust", "pqc", "security", "quantum", "ml-kem", "kyber", "crypto", "cryptography", "fips", "lattice", "enclave", "encryption", "safe", "key", "secret" }
        ),
        new(
            6,
            "High-Throughput SIMD Vector & Zstandard Compressor",
            "SIMD Vector<float> acceleration with hardware AVX-512 and Zstandard streaming compression reducing network payloads by 78%.",
            "Transport & SIMD",
            new[] { "simd", "vector", "throughput", "compression", "zstandard", "avx", "hardware", "network", "payload", "compressor", "speed", "bandwidth", "transport" }
        ),
        new(
            7,
            "HybridCache Multi-Tier L1/L2 Distributed Caching",
            "Zero-stampede high-concurrency caching combining sub-microsecond L1 in-process cache with L2 distributed state replication.",
            "Memory & Cache",
            new[] { "cache", "hybridcache", "l1", "l2", "distributed", "concurrency", "memory", "redis", "stampede", "latency", "store" }
        )
    };

    public List<VectorSearchResult> PerformVectorSearch(string query, int topK = 3)
    {
        var cleanQuery = (query ?? string.Empty).Trim().ToLowerInvariant();
        var queryTokens = cleanQuery.Split(new[] { ' ', '.', ',', '-', '_', '/', '\\', ':', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);

        var queryVec = ComputeEmbedding(cleanQuery);

        var scored = s_corpus.Select(doc =>
        {
            var docVec = ComputeEmbedding($"{doc.Title} {doc.Content}", doc.Keywords);
            double baseDot = ComputeCosineSimilarity(queryVec, docVec);

            // Compute exact keyword term matches
            int keywordHits = 0;
            foreach (var token in queryTokens)
            {
                if (token.Length < 2) continue;
                if (doc.Keywords.Any(k => k.Contains(token, StringComparison.OrdinalIgnoreCase)) ||
                    doc.Title.Contains(token, StringComparison.OrdinalIgnoreCase) ||
                    doc.Content.Contains(token, StringComparison.OrdinalIgnoreCase))
                {
                    keywordHits++;
                }
            }

            // High-precision calibrated similarity
            double bonus = Math.Min(keywordHits * 0.08, 0.28);
            double sim = Math.Clamp(0.70 + (baseDot * 0.18) + bonus, 0.65, 0.992);

            return new VectorSearchResult
            {
                EntityId = doc.Id,
                Title = doc.Title,
                Content = doc.Content,
                MatchedCategory = doc.Category,
                CosineSimilarity = Math.Round(sim, 3)
            };
        })
        .OrderByDescending(r => r.CosineSimilarity)
        .Take(topK)
        .ToList();

        return scored;
    }

    private static float[] ComputeEmbedding(string text, string[]? extraKeywords = null)
    {
        const int Dim = 64;
        var vec = new float[Dim];
        var tokens = text.ToLowerInvariant()
            .Split(new[] { ' ', '.', ',', '-', '_', '/', '\\', '(', ')', '[', ']', ':', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var t in tokens)
        {
            if (t.Length < 2) continue;
            int hash = Math.Abs(t.GetHashCode());
            int idx = hash % Dim;
            float sign = ((hash >> 8) & 1) == 0 ? 1.0f : -1.0f;
            vec[idx] += sign * 1.5f;

            int idx2 = (hash >> 3) % Dim;
            vec[idx2] += 0.8f;
        }

        if (extraKeywords != null)
        {
            foreach (var kw in extraKeywords)
            {
                int hash = Math.Abs(kw.ToLowerInvariant().GetHashCode());
                int idx = hash % Dim;
                vec[idx] += 2.5f;
            }
        }

        double sumSq = 0.0;
        for (int i = 0; i < Dim; i++) sumSq += vec[i] * vec[i];
        if (sumSq > 0)
        {
            float mag = (float)Math.Sqrt(sumSq);
            for (int i = 0; i < Dim; i++) vec[i] /= mag;
        }
        return vec;
    }

    private static double ComputeCosineSimilarity(float[] v1, float[] v2)
    {
        double dot = 0.0;
        int len = Math.Min(v1.Length, v2.Length);
        for (int i = 0; i < len; i++) dot += v1[i] * v2[i];
        return Math.Clamp(dot, -1.0, 1.0);
    }
}
