using System;
using System.Collections.Generic;

namespace Net11Showcase.Shared.Models;

#region 1. Blazor .NET 11 Models
public enum BlazorRenderModeKind
{
    StaticSsr,
    InteractiveServer,
    InteractiveWasm,
    AutoRenderMode,
    StreamingRendering
}

public sealed class QuickGridEnhancementItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public double Score { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime LastActivityUtc { get; set; } = DateTime.UtcNow;
}

public sealed class DynamicValidationFormModel
{
    public string FullName { get; set; } = string.Empty;
    public string CorporateEmail { get; set; } = string.Empty;
    public int ExperienceYears { get; set; } = 3;
    public string ClearanceCode { get; set; } = string.Empty;
    public bool AcceptTerms { get; set; } = false;
}
#endregion

#region 2. MAUI Blazor Hybrid Models
public sealed class CoreClrRuntimeMetrics
{
    public string RuntimeVersion { get; set; } = ".NET 11.0.0-rc.3 (CoreCLR / ReadyToRun)";
    public double StartupTimeMs { get; set; } = 84.2;
    public double MemoryWorkingSetMb { get; set; } = 38.6;
    public double CpuUsagePercent { get; set; } = 1.4;
    public int GcCollectionsCount { get; set; } = 2;
    public bool JitTieredPgoActive { get; set; } = true;
    public string HardwarePlatform { get; set; } = "ARM64 / x64 Universal";
}

public enum DeviceSensorKind
{
    GpsLocation,
    Accelerometer,
    Gyroscope,
    CompassHeading,
    BatteryLevel,
    CameraFeed,
    BiometricTouch
}

public sealed class DeviceSensorReading
{
    public DeviceSensorKind Kind { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}

public sealed class OfflineSyncQueueItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string EntityName { get; set; } = "CustomerRecord";
    public string Action { get; set; } = "UPSERT";
    public string PayloadJson { get; set; } = "{}";
    public DateTime QueuedAt { get; set; } = DateTime.UtcNow;
    public string SyncStatus { get; set; } = "Synced (Local SQLite -> Cloud DB)";
}
#endregion

#region 3. Backend Minimal API & SignalR Models
public sealed class ApiEndpointDescriptor
{
    public string Route { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = "GET";
    public string Summary { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public bool ResponseStreaming { get; set; }
    public bool RequiresAuth { get; set; }
}

public sealed class SignalRTelemetryMetric
{
    public string NodeId { get; set; } = "node-us-east-1";
    public double ThroughputRps { get; set; } = 28450.0;
    public double LatencyP99Ms { get; set; } = 1.12;
    public int ActiveConnections { get; set; } = 1420;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
#endregion

#region 4. EF Core 11 Models
public sealed class CustomerEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Tier { get; set; } = "Enterprise Tier 1";
    public string MetadataJson { get; set; } = "{}";
    public float[] EmbeddingVector { get; set; } = Array.Empty<float>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class VectorSearchResult
{
    public int EntityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public double CosineSimilarity { get; set; }
    public string MatchedCategory { get; set; } = string.Empty;
}
#endregion

#region 5. Native AOT & Benchmark Lab Models
public sealed class BenchmarkComparisonResult
{
    public string FrameworkVersion { get; set; } = ".NET 11";
    public string RuntimeMode { get; set; } = "Native AOT";
    public double StartupTimeMs { get; set; }
    public double ThroughputKiloRps { get; set; }
    public double MemoryWorkingSetMb { get; set; }
    public double PublishSizeMb { get; set; }
    public double WasmLoadTimeMs { get; set; }
    public string EfficiencyRating { get; set; } = "A+ (Optimal)";
}
#endregion

#region 6. AI Showcase Models
public sealed class AiChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Role { get; set; } = "Assistant";
    public string Content { get; set; } = string.Empty;
    public int TokensUsed { get; set; } = 42;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsStreaming { get; set; }
}

public sealed class RagDocumentChunk
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string DocumentTitle { get; set; } = string.Empty;
    public string ChunkText { get; set; } = string.Empty;
    public double RelevanceScore { get; set; } = 0.94;
    public string SourceSection { get; set; } = "Architecture";
}
#endregion
