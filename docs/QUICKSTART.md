# 🚀 Net11 Showcase Suite -- Developer Quickstart Guide

This guide will help you install, build, run, and explore the **Net11 Showcase Suite** on Windows or in cross-platform container environments.

---

## 📋 Prerequisites

1. **Operating System**: Windows 10/11, macOS, or Linux (Ubuntu 22.04+).
2. **.NET SDK**: .NET 11.0 Preview / RC SDK (tested with `11.0.100-rc.3.26522.148` or higher).
3. **PowerShell 7+ or Windows PowerShell 5.1** (for pre-flight launcher and installer scripts).
4. **Browser**: Edge, Chrome, Firefox, or Safari.

---

## ⚡ Method 1: Double-Click Desktop Installer (Fastest)

1. Navigate to the root of the repository.
2. Double-click `Install-Net11Showcase.cmd`.
3. The installer will:
   - Extract and install the Release binaries to `%LOCALAPPDATA%\Net11Showcase`.
   - Create a desktop shortcut: **Net11 Showcase Suite**.
   - Create a Start Menu shortcut under **Net11 Showcase**.
   - Generate an uninstaller: `%LOCALAPPDATA%\Net11Showcase\Uninstall-Net11Showcase.cmd`.
   - Prompt you to launch immediately.

---

## 🛠️ Method 2: Running from Source with Port Coordinator

To run from source code with automatic port discovery:

```powershell
# Run the PowerShell port coordinator (scans ports 5150-5250)
.\launch-showcase.ps1
```

Or using the Command Prompt:
```cmd
launch-showcase.cmd
```

### Coordinator Flags
- `-StartPort <int>`: Starting TCP port to scan (default: `5150`).
- `-MaxPortScan <int>`: Maximum ports to probe (default: `100`).
- `-NoBrowser`: Prevents launching the browser automatically (useful in CI/CD).

---

## 💻 Method 3: .NET CLI Direct Build & Run

```bash
# 1. Restore and build entire solution
dotnet build Net11Showcase.slnx

# 2. Run unit and integration tests
dotnet test Net11Showcase.slnx

# 3. Launch the Web Host directly
dotnet run --project src/Net11Showcase.Web/Net11Showcase.Web/Net11Showcase.Web.csproj --urls "http://127.0.0.1:5150"
```

Once running, navigate to:
- **Interactive UI**: `http://127.0.0.1:5150/`
- **Scalar OpenAPI Console**: `http://127.0.0.1:5150/scalar/v1`

---

## ☁️ Cloud Configuration Options

The suite runs 100% free locally, but can be switched to cloud services in seconds:

### 1. Azure App Service (F1 Free Tier)
Deploy at strictly **$0.00** monthly cost:
```bash
az webapp up --name net11-showcase-suite --resource-group rg-showcase --sku F1 --os-type Linux
```

### 2. Azure SQL Database
In `src/Net11Showcase.Api/appsettings.json`, set:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=ShowcaseDb;Persist Security Info=False;User ID=youradmin;Password=yourpass;Encrypt=True;"
  }
}
```

### 3. Azure OpenAI Service
In `src/Net11Showcase.Api/appsettings.json`, set:
```json
{
  "AzureOpenAi": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "YOUR_AZURE_OPENAI_KEY",
    "DeploymentName": "gpt-4o"
  }
}
```
