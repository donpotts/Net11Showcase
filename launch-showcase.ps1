<#
.SYNOPSIS
    Net11Showcase -- Standalone .NET 11 Showcase Suite Launcher
.DESCRIPTION
    1. Scans and allocates an unoccupied port (5150-5250) to prevent port collisions.
    2. Boots the Net11Showcase.Web host process.
    3. Runs active pre-flight health polling loop until HTTP 200 is verified.
    4. Opens the browser UI only after the server is 100% online.
    5. Clean process termination on exit.
#>

param(
    [int]$StartPort = 5150,
    [int]$MaxPortScan = 100,
    [switch]$NoBrowser
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "   .NET 11 Showcase Platform & Enterprise Reference Lab          " -ForegroundColor White
Write-Host "   ASP.NET Core • Blazor • MAUI Hybrid • EF Core 11 • Native AOT " -ForegroundColor Yellow
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Port Discovery
Write-Host "[1/3] Scanning for available TCP ports starting at $StartPort..." -ForegroundColor Gray

function Test-PortAvailable([int]$port) {
    try {
        $listener = New-Object System.Net.Sockets.TcpListener([System.Net.IPAddress]::Loopback, $port)
        $listener.Start()
        $listener.Stop()
        return $true
    }
    catch {
        return $false
    }
}

$allocatedPort = $null
for ($p = $StartPort; $p -lt ($StartPort + $MaxPortScan); $p++) {
    if (Test-PortAvailable $p) {
        $allocatedPort = $p
        break
    }
}

if (-not $allocatedPort) {
    Write-Error "CRITICAL: Could not find any free port between $StartPort and $($StartPort + $MaxPortScan)."
    exit 1
}

Write-Host "   -> Allocated Free Port: $allocatedPort" -ForegroundColor Green
$targetUrl = "http://127.0.0.1:$allocatedPort"

# 2. Start Web Host
Write-Host "[2/3] Launching Net11Showcase.Web host on $targetUrl..." -ForegroundColor Gray
$projectPath = Join-Path $ScriptDir "src\Net11Showcase.Web\Net11Showcase.Web\Net11Showcase.Web.csproj"
$exePath = if (Test-Path (Join-Path $ScriptDir "Net11Showcase.Web.exe")) {
    Join-Path $ScriptDir "Net11Showcase.Web.exe"
} elseif (Test-Path (Join-Path $ScriptDir "dist\web\Net11Showcase.Web.exe")) {
    Join-Path $ScriptDir "dist\web\Net11Showcase.Web.exe"
} else {
    $null
}

$psi = New-Object System.Diagnostics.ProcessStartInfo
if ($exePath) {
    Write-Host "   -> Using standalone binary: $exePath" -ForegroundColor Green
    $psi.FileName = $exePath
    $psi.Arguments = "--urls `"$targetUrl`""
    $psi.WorkingDirectory = Split-Path -Parent $exePath
} else {
    Write-Host "   -> Using dotnet run from source: $projectPath" -ForegroundColor Gray
    $psi.FileName = "dotnet"
    $psi.Arguments = "run --project `"$projectPath`" --urls `"$targetUrl`""
    $psi.WorkingDirectory = $ScriptDir
}
$psi.UseShellExecute = $false
$psi.CreateNoWindow = $true
$serverProcess = [System.Diagnostics.Process]::Start($psi)

# 3. Pre-Flight Health Probe
Write-Host "[3/3] Executing Pre-Flight Health Probe on $targetUrl..." -ForegroundColor Yellow

$probeSuccess = $false
$maxAttempts = 35
$probeStopwatch = [System.Diagnostics.Stopwatch]::StartNew()

for ($i = 1; $i -le $maxAttempts; $i++) {
    Write-Host -NoNewline "`r   -> Health Probe Attempt $i/$maxAttempts... " -ForegroundColor Gray
    try {
        $req = [System.Net.WebRequest]::Create($targetUrl)
        $req.Timeout = 1200
        $resp = $req.GetResponse()
        if ($resp.StatusCode -eq [System.Net.HttpStatusCode]::OK) {
            $resp.Close()
            $probeSuccess = $true
            $probeStopwatch.Stop()
            break
        }
        $resp.Close()
    }
    catch {}
    Start-Sleep -Milliseconds 500
}

Write-Host ""
if (-not $probeSuccess) {
    Write-Host "[-] Pre-flight probe timed out after $($probeStopwatch.Elapsed.TotalSeconds.ToString('F1'))s." -ForegroundColor Red
    if ($serverProcess -and -not $serverProcess.HasExited) { $serverProcess.Kill() }
    exit 1
}

Write-Host "[+] .NET 11 WEB HOST VERIFIED 100% ONLINE!" -ForegroundColor Green
Write-Host "    Target URL: $targetUrl" -ForegroundColor Cyan
Write-Host ""

if (-not $NoBrowser) {
    Write-Host ">>> Launching Showcase UI in Default Browser..." -ForegroundColor Green
    Start-Process $targetUrl
}

Write-Host ""
Write-Host "Net11Showcase is actively running. Press Ctrl+C to terminate." -ForegroundColor White
Write-Host "-----------------------------------------------------------------" -ForegroundColor Gray

try {
    while ($true) {
        if ($serverProcess.HasExited) { break }
        Start-Sleep -Seconds 1
    }
}
finally {
    if ($serverProcess -and -not $serverProcess.HasExited) {
        Write-Host "`n[*] Stopping web host process ($($serverProcess.Id))..." -ForegroundColor Yellow
        $serverProcess.Kill()
        Write-Host "[+] Process stopped cleanly. Port $allocatedPort released." -ForegroundColor Green
    }
}
