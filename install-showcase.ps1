<#
.SYNOPSIS
    Installs the .NET 11 Showcase Platform & Enterprise Architecture Lab to the local machine.
.DESCRIPTION
    Extracts/copies the standalone .NET 11 release to %LOCALAPPDATA%\Net11Showcase,
    configures the pre-flight port coordinator launcher, creates Windows Desktop and
    Start Menu shortcuts, and generates an uninstaller.
#>

param(
    [string]$InstallPath = (Join-Path $env:LOCALAPPDATA "Net11Showcase"),
    [switch]$NoShortcuts,
    [switch]$Silent
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "   .NET 11 Showcase Platform -- Desktop & Server Installer       " -ForegroundColor White
Write-Host "   Enterprise Architecture Lab with Pre-Flight Port Coordinator  " -ForegroundColor Yellow
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "[1/5] Target Installation Path: $InstallPath" -ForegroundColor Gray
if (-not (Test-Path $InstallPath)) {
    New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null
    Write-Host "   -> Created installation directory." -ForegroundColor Green
}

# Determine source binary directory
$sourceDir = if (Test-Path (Join-Path $ScriptDir "dist\web\Net11Showcase.Web.exe")) {
    Join-Path $ScriptDir "dist\web"
} else {
    Write-Host "   -> Pre-compiled binary not found. Compiling Release distribution now..." -ForegroundColor Yellow
    $distPath = Join-Path $ScriptDir "dist\web"
    $proj = Join-Path $ScriptDir "src\Net11Showcase.Web\Net11Showcase.Web\Net11Showcase.Web.csproj"
    & dotnet publish $proj -c Release -o $distPath | Out-Null
    $distPath
}

Write-Host "[2/5] Copying .NET 11 Showcase Assets from $sourceDir..." -ForegroundColor Gray
Copy-Item -Path "$sourceDir\*" -Destination $InstallPath -Recurse -Force

# Copy launchers and scripts
$auxiliaryFiles = @(
    (Join-Path $ScriptDir "launch-showcase.ps1"),
    (Join-Path $ScriptDir "launch-showcase.cmd"),
    (Join-Path $ScriptDir "README.md")
)

foreach ($f in $auxiliaryFiles) {
    if (Test-Path $f) {
        Copy-Item -Path $f -Destination $InstallPath -Force
        Write-Host "   -> Installed coordinator tool: $(Split-Path -Leaf $f)" -ForegroundColor Gray
    }
}

# Generate dedicated uninstaller
Write-Host "[3/5] Generating uninstaller..." -ForegroundColor Gray
$uninstallerPath = Join-Path $InstallPath "Uninstall-Net11Showcase.cmd"
$desktopLnk = Join-Path ([Environment]::GetFolderPath("Desktop")) "Net11 Showcase Suite.lnk"
$startMenuDir = Join-Path ([Environment]::GetFolderPath("Programs")) "Net11 Showcase"
$startMenuLnk = Join-Path $startMenuDir "Net11 Showcase Suite.lnk"

$uninstallScript = @"
@echo off
title Uninstalling .NET 11 Showcase Suite
echo Removing .NET 11 Showcase Suite...
if exist "$desktopLnk" del /f /q "$desktopLnk"
if exist "$startMenuLnk" del /f /q "$startMenuLnk"
if exist "$startMenuDir" rmdir /s /q "$startMenuDir"
echo Removing installation files...
cd /d "%LOCALAPPDATA%"
rmdir /s /q "$InstallPath"
echo Uninstall completed successfully.
pause
"@
Set-Content -Path $uninstallerPath -Value $uninstallScript -Encoding UTF8

# Create Shortcuts
if (-not $NoShortcuts) {
    Write-Host "[4/5] Creating Windows Desktop and Start Menu Shortcuts..." -ForegroundColor Gray
    $wshShell = New-Object -ComObject WScript.Shell

    $targetCmd = Join-Path $InstallPath "launch-showcase.cmd"
    $iconPath = Join-Path $InstallPath "wwwroot\favicon.ico"
    if (-not (Test-Path $iconPath)) {
        $iconPath = Join-Path $InstallPath "Net11Showcase.Web.exe"
    }

    # 1. Desktop Shortcut
    $shortcutDesktop = $wshShell.CreateShortcut($desktopLnk)
    $shortcutDesktop.TargetPath = $targetCmd
    $shortcutDesktop.WorkingDirectory = $InstallPath
    $shortcutDesktop.Description = ".NET 11 Showcase Platform & Enterprise Architecture Lab"
    if (Test-Path $iconPath) {
        $shortcutDesktop.IconLocation = "$iconPath,0"
    }
    $shortcutDesktop.Save()
    Write-Host "   -> Created Desktop Shortcut: $desktopLnk" -ForegroundColor Green

    # 2. Start Menu Shortcut
    if (-not (Test-Path $startMenuDir)) {
        New-Item -ItemType Directory -Path $startMenuDir -Force | Out-Null
    }
    $shortcutStart = $wshShell.CreateShortcut($startMenuLnk)
    $shortcutStart.TargetPath = $targetCmd
    $shortcutStart.WorkingDirectory = $InstallPath
    $shortcutStart.Description = ".NET 11 Showcase Platform & Enterprise Architecture Lab"
    if (Test-Path $iconPath) {
        $shortcutStart.IconLocation = "$iconPath,0"
    }
    $shortcutStart.Save()
    Write-Host "   -> Created Start Menu Shortcut: $startMenuLnk" -ForegroundColor Green
}

Write-Host "[5/5] Finalizing installation..." -ForegroundColor Gray
Write-Host ""
Write-Host "=================================================================" -ForegroundColor Green
Write-Host "   INSTALLATION COMPLETE!                                        " -ForegroundColor Green
Write-Host "   Installed to: $InstallPath                                    " -ForegroundColor White
Write-Host "   Desktop and Start Menu shortcuts created.                     " -ForegroundColor White
Write-Host "   Launch using: $InstallPath\launch-showcase.cmd                " -ForegroundColor Cyan
Write-Host "=================================================================" -ForegroundColor Green
Write-Host ""
