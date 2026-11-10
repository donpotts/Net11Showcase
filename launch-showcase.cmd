@echo off
setlocal
cd /d "%~dp0"
title .NET 11 Showcase Platform Launcher

echo =================================================================
echo    .NET 11 Showcase Platform ^& Enterprise Reference Lab
echo    Port-Guaranteed Launcher with Pre-Flight Health Probe
echo =================================================================
echo.

:: Automatically unblock downloaded archive files if blocked by Windows Mark of the Web
powershell -NoProfile -ExecutionPolicy Bypass -Command "Get-ChildItem -Path '%~dp0' -Recurse -ErrorAction SilentlyContinue | Unblock-File -ErrorAction SilentlyContinue" 2>nul

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0launch-showcase.ps1" %*

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [!] Launcher exited with error code %ERRORLEVEL%.
    echo [!] Press any key to close this terminal...
    pause >nul
)
