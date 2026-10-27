@echo off
setlocal
cd /d "%~dp0"
title .NET 11 Showcase Platform -- Desktop & Server Installer

echo =================================================================
echo    .NET 11 Showcase Platform & Enterprise Reference Lab
echo    Windows Installer with Pre-Flight Port Coordinator Setup
echo =================================================================
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0install-showcase.ps1" %*

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [!] Installation encountered an error (Code: %ERRORLEVEL%).
    pause
    exit /b %ERRORLEVEL%
)

echo.
set /p LAUNCH="Would you like to launch the .NET 11 Showcase Suite now? (Y/N): "
if /i "%LAUNCH%"=="Y" (
    powershell -NoProfile -ExecutionPolicy Bypass -File "%LOCALAPPDATA%\Net11Showcase\launch-showcase.ps1"
)
