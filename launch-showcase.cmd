@echo off
setlocal
cd /d "%~dp0"
title .NET 11 Showcase Platform Launcher

echo =================================================================
echo    .NET 11 Showcase Platform ^& Enterprise Reference Lab
echo    Port-Guaranteed Launcher with Pre-Flight Health Probe
echo =================================================================
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0launch-showcase.ps1" %*

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [!] Launcher exited with error code %ERRORLEVEL%.
    pause
)
