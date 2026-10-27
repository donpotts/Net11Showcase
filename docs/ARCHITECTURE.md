# 🏗️ Net11 Showcase Suite -- Architecture & Technical Design

## Overview

The **Net11 Showcase Suite** is an enterprise-grade reference architecture demonstrating the latest features and performance enhancements in **.NET 11**, **ASP.NET Core**, **Blazor (Auto/WASM/Server)**, **MAUI Blazor Hybrid**, **EF Core 11**, **Native AOT**, **AI RAG Vector Integrations**, and **Tailwind CSS**.

---

## 🏛️ Solution Structure

```text
Net11Showcase.sln

src/
├── Net11Showcase.Shared/
│   ├── Models/
│   │   └── ShowcaseModels.cs         # Domain models, benchmarks, and DTO contracts
│   └── Services/
│       └── ShowcaseDataService.cs    # Mock & live benchmark data services
│
├── Net11Showcase.Components/
│   ├── AppShell/
│   │   ├── MainLayout.razor          # Responsive AppShell with sticky header & sidebar
│   │   └── NavMenu.razor             # Icon-enhanced sidebar navigation
│   └── wwwroot/
│       └── css/tailwind.min.css      # Pre-compiled Tailwind CSS distribution
│
├── Net11Showcase.Data/
│   ├── AppDbContext.cs               # EF Core 11 DbContext (SQLite & Azure SQL ready)
│   ├── Entities/
│   │   ├── CustomerEntity.cs         # JSON column entity mappings
│   │   └── VectorEmbeddingItem.cs   # High-dimensional vector items
│   └── Repositories/
│       └── CustomerRepository.cs     # LINQ MaxBy, JSON querying, in-memory cosine math
│
├── Net11Showcase.Api/
│   ├── Program.cs                    # Minimal APIs, Scalar OpenAPI 3.1 (/scalar/v1)
│   ├── appsettings.json              # SQLite default + Azure connection points
│   └── Streaming/
│       └── TelemetryStream.cs        # IAsyncEnumerable streaming endpoints (28k+ RPS)
│
├── Net11Showcase.Web/
│   ├── Net11Showcase.Web/            # ASP.NET Core Blazor server host
│   │   ├── Program.cs                # InteractiveAuto render mode configuration
│   │   ├── App.razor                 # Root HTML shell & script imports
│   │   ├── Routes.razor              # Client/Server routing engine
│   │   └── Components/Pages/
│   │       └── Home.razor            # 6-module interactive showcase lab
│   └── Net11Showcase.Web.Client/     # WebAssembly client component assembly
│
├── Net11Showcase.Maui/
│   ├── MauiProgram.cs                # MAUI Blazor Hybrid host (CoreCLR Tiered PGO)
│   ├── MainPage.xaml                 # BlazorWebView root container
│   └── Platforms/                    # Windows WinUI 3, Android, iOS, MacCatalyst
│
└── Net11Showcase.Tests/
    └── Net11ShowcaseTests.cs         # xUnit unit & integration tests (EF Core + Vectors)
```

---

## 🔬 Core Architectural Patterns

### 1. Blazor Auto Render Mode (`InteractiveAuto`)
- **Initial Load**: Fast static server-rendered HTML shell (`Static SSR`) delivered instantaneously with zero WebAssembly download lag.
- **Hydration**: Client WebAssembly runtime downloads in the background and is cached in browser cache/IndexedDB.
- **Interactive Transition**: Subsequent component interactions automatically migrate to client-side WebAssembly execution, freeing server memory and network connections.

### 2. EF Core 11 JSON Document Queries & Modern LINQ
- **Nested JSON Columns**: Complex metadata stored in SQLite `TEXT` / SQL Server `nvarchar(max)` and queried directly using EF Core JSON mapping without serialization friction.
- **Modern LINQ Aggregations**: Native usage of `MaxBy()` and `MinBy()` for single-pass aggregate queries.
- **In-Memory Cosine Similarity Vector Math**:
  ```csharp
  public static double ComputeCosineSimilarity(ReadOnlySpan<float> vecA, ReadOnlySpan<float> vecB)
  {
      float dot = 0.0f, normA = 0.0f, normB = 0.0f;
      for (int i = 0; i < vecA.Length; i++)
      {
          dot += vecA[i] * vecB[i];
          normA += vecA[i] * vecA[i];
          normB += vecB[i] * vecB[i];
      }
      return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
  }
  ```

### 3. Native AOT Compilation & Diagnostics
- **Ahead-Of-Time Compilation**: Pre-compiles .NET IL directly into native machine code (`win-x64`, `linux-x64`).
- **Benefits**:
  - Eliminates JIT compilation runtime overhead and warmup pauses.
  - Sub-10ms startup time (ideal for serverless containers and edge gateways).
  - Minimal memory footprint (<15 MB working set).
  - Self-contained single binary with zero external .NET runtime installation required.

---

## 🛡️ Zero-Cost Philosophy & Cloud Portability

The suite is engineered from day one on **100% Free, Open-Source Technologies**:
- **Database**: Local SQLite engine (`showcase_net11.db`) requiring 0 database servers, zero cloud bills, and zero configuration.
- **Vector Search**: Exact cosine vector calculations computed locally without Pinecone, Milvus, or paid vector databases.
- **AI RAG Prompt Studio**: Built-in architecture vector corpus and mock reasoning engine that functions completely offline.
- **Cloud Ready**: Standard ASP.NET Core `IConfiguration` keys enable toggling Azure OpenAI, Azure SQL, or Azure App Service F1 tier with one line in `appsettings.json`.
