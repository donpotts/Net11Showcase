# 🧩 Reusable Tailwind Component Library (`Net11Showcase.Components`)

The suite provides a clean, dependency-free Razor Class Library (`Net11Showcase.Components`) adapted from the **BlazorTailwindUI** ecosystem.

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
