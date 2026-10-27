# ❓ Net11 Showcase Suite -- Help, Troubleshooting & FAQ

This document addresses common questions, troubleshooting steps, and configuration help for the **Net11 Showcase Suite**.

---

## 🔍 Frequently Asked Questions (FAQ)

### Q: Does this require any paid cloud licenses or API keys?
**No.** The Net11 Showcase Suite is 100% free and open source. It uses local SQLite, in-memory cosine vector similarity math, and local dev servers. No paid APIs or accounts are required.

### Q: Can I run this without installing .NET 11 globally?
**Yes.** If you use `Install-Net11Showcase.cmd`, it installs the pre-compiled Release distribution to `%LOCALAPPDATA%\Net11Showcase` which includes all compiled assemblies.

### Q: How does the port coordinator prevent port conflicts?
The port coordinator (`launch-showcase.ps1`) automatically scans TCP ports starting at `5150` up to `5250`. It binds to an unoccupied port, starts the host, and probes the health endpoint with HTTP `GET` requests until `HTTP 200 OK` is verified before opening the browser.

---

## 🛠️ Troubleshooting Guide

### 1. Port In Use / Collision Errors
- **Symptom**: `Failed to bind to address http://127.0.0.1:5150: address already in use`.
- **Solution**: Run `.\launch-showcase.ps1` without arguments; it automatically scans for free ports. Alternatively, specify a custom port:
  ```powershell
  .\launch-showcase.ps1 -StartPort 5200
  ```

### 2. PowerShell Execution Policy Restriction
- **Symptom**: `launch-showcase.ps1 cannot be loaded because running scripts is disabled on this system`.
- **Solution**: Double-click `launch-showcase.cmd` (or `Install-Net11Showcase.cmd`), which runs PowerShell with `-ExecutionPolicy Bypass`. Alternatively, run in PowerShell:
  ```powershell
  powershell -ExecutionPolicy Bypass -File .\launch-showcase.ps1
  ```

### 3. Windows Firewall Prompt
- **Symptom**: Windows Firewall pop-up asking to allow `dotnet.exe` or `Net11Showcase.Web.exe`.
- **Solution**: Click **Allow Access** for Private Networks. The server only binds to loopback (`127.0.0.1`).

### 4. Database Reset
- **Symptom**: You want to reset the SQLite database to factory state.
- **Solution**: Delete the file `showcase_net11.db` in the app directory. On next startup, the database is automatically recreated and seeded with fresh sample data.

---

## 📞 Getting Help & Reporting Issues

- **Repository**: [Net11 Showcase on GitHub](https://github.com/donpotts/BlazorTailwindUI)
- **License**: MIT Open Source License
