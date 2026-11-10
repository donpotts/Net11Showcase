# 🧩 Reusable Tailwind Component Library (`Net11Showcase.Components`)

The suite provides a clean, dependency-free Razor Class Library (`Net11Showcase.Components`) adapted from the **BlazorTwUI** ecosystem.

---

## 🛡️ Agent Ops (`Net11Showcase.Components/AgentOps`)

- **`TwAgentConsole.razor`**: Complete human-in-the-loop workspace composing the pieces below around an `AgentRunSession`. Register an `IAgentTransport` (default: offline `SimulatedAgentTransport`).
- **`TwApprovalGate.razor`**: Blocking approval card for pending high-risk tool calls; approve with edited, validated JSON arguments or reject with a reason.
- **`TwToolCallCard.razor`**: Collapsible tool call with streamed arguments, result, duration, risk level and a text status.
- **`TwTakeoverBar.razor`**: Run status with Take over, Hand back and Stop.
- **`TwAgentStateInspector.razor`**: Live shared state; editable while the human is in control.
- **`TwAuditTrail.razor`**: Filterable audit log with JSON export (JS module `_content/Net11Showcase.Components/agentOps.js`).
- **`TwAgentRoster.razor`**: Accessible agent list with role and status.

Styles: these components use Tailwind utilities compiled by `npm run build:agentops-css` into `wwwroot/css/agent-ops.css`.

---

## 🎨 Component Catalog

### 1. Layout & Shell Components
- **`MainLayout.razor`**: Responsive AppShell featuring sticky glassmorphic navigation header, collapsable sidebar, breadcrumb bar, and mobile responsive drawer.
- **`NavMenu.razor`**: Multi-section navigation menu with active route tracking, badges, and icon indicators.

### 2. Data & Grid Components
- **`QuickGridEnhancement`**: High-performance grid wrapper featuring:
  - Instant client-side search filtering.
  - Multi-column sort indicators.
  - Row click selection with detail inspection drawer.
  - Dynamic status badge formatting.

### 3. Device & Mobile Bridge Components
- **`SensorDiagnosticsCard`**: Real-time 3-axis accelerometer and gyroscope visualization.
- **`GpsGeofenceWidget`**: Live latitude/longitude coordinate monitor with inside/outside security boundary detection.
- **`OfflineSyncQueueViewer`**: SQLite background sync queue visualization with transaction states (`INSERT`, `UPDATE`, `UPSERT`).

### 4. AI & Chat Components
- **`AiChatAssistant`**: Multi-turn chat interface with streaming message bubbles, operator/assistant role tags, timestamp formatting, and prompt suggestions.
- **`VectorSearchExplorer`**: Interactive semantic query input with dynamic cosine similarity progress bars and matched category tags.

### 5. Benchmark & Diagnostics Components
- **`BenchmarkMatrixTable`**: Color-coded performance comparison table comparing .NET 8 through .NET 11 Native AOT metrics.
- **`RuntimeTelemetryHud`**: Glassmorphic KPI tiles displaying startup time, memory working set, throughput, and vector similarity in real time.

### 6. BlazorTwUI Design System (`Net11Showcase.Components/UI`)
- **`TwBadge`**: Semantic badges and pill indicators with dot adornments.
- **`TwButton`**: Multi-variant interactive buttons with async loading spinner, disabled state, and icon slots.
- **`TwCard`**: Surface container with configurable header, body, footer, padding, and elevation.
- **`TwStatCard`**: KPI metrics card with trend pill badge, icon slot, and subtext description.
- **`TwAlert`**: Dismissible and contextual message boxes with semantic color themes.
- **`TwModal`**: Accessible dialog overlay with backdrop dismiss, responsive sizing, and action slots.
- **`TwCodeBlock`**: Formatted syntax-highlighted code block with language indicator, filename tab, and one-click copy feedback.
- **`TwCodeDiff`**: Split and unified code change diffing.
- **`TwTabs`**: Accessible tab strip with pill and underline indicators.
- **`TwProgressBar`**: Determinate and indeterminate progress visualization.
- **`TwPill`**: Lightweight tag component.
- **`TwKbd`**: Accessible keyboard key indicator.

---

## 💼 Showcase Application Pages

### 1. Enterprise Contract Signer (`ContractSigner.razor`)
- **Route**: `/contract-signer`
- **Features**:
  - Interactive signature canvas with pointer and touch event capture.
  - Cryptographic document hash calculation (SHA-256 seal).
  - Post-Quantum Cryptography (NIST FIPS 203 ML-KEM-768 Kyber) key exchange and dynamic key rotation.
  - In-memory PDF byte generation, document flattening, and browser download trigger.
  - BlazorTwUI `<TwButton>`, `<TwAlert>`, and `<TwBadge>` integration.

### 2. Fintech Cockpit (`FintechCockpit.razor`)
- **Route**: `/fintech-cockpit`
- **Features**:
  - Live EF Core 11 JSON Column Queries with client-side filters.
  - NACHA 94-character fixed-width ACH transaction dispatch and format validation.
  - Post-Quantum Cryptographic Key Encapsulation (NIST FIPS 203 ML-KEM-768 Kyber) rotation simulation with entropy bars.

### 3. Hardware Motion Lab (`HardwareLab.razor`)
- **Route**: `/hardware-lab`
- **Features**:
  - Kinetic 3D tilt box driven by real-time simulated 3-axis Gyroscope & Accelerometer math.
  - Live GPS coordinate proximity radar with dynamic geofencing boundary calculation.
  - Performance benchmark HUD comparing .NET 8, 9, 10, and 11 GA.

### 4. RAG Semantic Radar (`RagRadar.razor`)
- **Route**: `/rag-radar`
- **Features**:
  - Signed feature hashing vector projection (`HashingEmbedder`).
  - In-memory and EF Core 11 Cosine Similarity nearest-neighbor vector matcher.
  - Live streaming AI response token generator with token-per-second telemetry.
  - BlazorTwUI `<TwStatCard>`, `<TwButton>`, and `<TwBadge>` integration.

### 5. .NET 11 Features & Architectural Roadmap Lab (`Net11Features.razor`)
- **Route**: `/net11-features`
- **Features**:
  - Complete 8-pillar architectural interactive matrix (C# 15, ASP.NET Core, EF Core 11, Runtime, Libraries, SDK, Windows Forms, .NET MAUI).
  - Full-screen high-res architectural infographic modal powered by `<TwModal>`.
  - Live C# 15 Discriminated Union pattern matching benchmark lab (zero allocations vs exceptions).
  - Live hardware SIMD vector acceleration benchmark with GFLOPS and microsecond telemetry.
  - Code blocks rendered with BlazorTwUI `<TwCodeBlock>`.

