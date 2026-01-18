Software Requirements Specification (SRS)
Project: Satark
Version: 1.0
Date: January 18, 2026

---

# 1. Introduction

## 1.1 Purpose
This document defines the functional and non-functional requirements for a Windows background service ("Satark") that monitors system boot events and delivers a push notification to a mobile device via ntfy.sh. This SRS focuses on behavior, deployment, monitoring, and testing of the headless service.

## 1.2 Intended Audience
- Developers implementing the service
- System administrators installing and operating the service
- QA engineers creating automated and manual tests
- Security reviewers and release managers

## 1.3 Product Scope
A lightweight headless Windows Service that:
- Starts automatically on boot (LocalSystem)
- Waits briefly for network stabilization
- Sends a POST notification to ntfy.sh with a configurable topic and metadata
- Logs results to Windows Event Viewer and supports retries

---

# 2. Overall Description

## 2.1 User Needs (Visual summary)
- Immediate, automated alert when a machine completes boot and obtains network connectivity
- Minimal configuration and maintenance
- Clear, auditable logs in Event Viewer
- Configurable retry behavior for transient networks

## 2.2 Assumptions & Dependencies
- Internet connectivity to reach ntfy.sh
- Windows 10/11 or Windows Server 2019+ (service host)
- .NET 8 runtime installed on host
- ntfy.sh availability (third-party API)

---

# 3. Process Flow (visual + collapsible details)

```mermaid
flowchart LR
  A[Boot / SCM starts service] --> B[Stabilization: wait 30s]
  B --> C{Network available?}
  C -- Yes --> D[Send async POST to https://ntfy.sh/{topic}]
  C -- No --> E[Retry up to 3 times w/ backoff]
  D --> F[Log Success to Event Viewer]
  E --> F
  F --> G[Enter low-resource idle loop]
```

<details>
<summary>Detailed stage table</summary>

| Stage | Name | Description |
|---:|---|---|
| I | Boot Trigger | Windows SCM executes the binary with LocalSystem privileges. |
| II | Stabilization | Service waits (default 30s) to ensure network drivers are initialized. Configurable via settings. |
| III | Web Request | Asynchronous POST to https://ntfy.sh/{topic} with headers (Title, Priority). |
| IV | Heartbeat | Log "Success" (or error) in Event Viewer and enter idle loop, waiting for next boot or restart. |

</details>

---

# 4. System Requirements & Tools

## 4.1 Development Environment
- IDE: Visual Studio 2022 or VS Code
- Framework: .NET 8.0 SDK (Worker Service Template)
- Language: C# 12
- Libraries: Microsoft.Extensions.Hosting.WindowsServices, Microsoft.Extensions.Logging, System.Net.Http

## 4.2 Production Environment
- Runtime: .NET 8 Runtime (Desktop or Server)
- Install: Administrative privileges required to create/run service
- Path: Recommended installation directory: C:\Program Files\Satark\

---

# 5. Specific Requirements (structured & actionable)

## 5.1 External API Requirements (ntfy)
- Provider: ntfy.sh (HTTP)
- Endpoint: POST https://ntfy.sh/{topic}
- Required headers (example):
  - Title: System Alert
  - Priority: high
- Body: customizable, includes hostname, timestamp, optional IP address

### Example HTTP request
```http
POST /my-topic HTTP/1.1
Host: ntfy.sh
Title: System Alert
Priority: high
Content-Type: text/plain

Boot completed on HOSTNAME (192.0.2.1) at 2026-01-18T10:00:00Z
```

## 5.2 Functional Requirements (FRs)
- FR1: Service shall automatically start within 2 minutes of system power-on (installed start=auto).
- FR2: Service shall retry the notification up to 3 times on network failure (configurable count).
- FR3: Service shall log each attempt with timestamp and result to Application Event Log.
- FR4: Service shall expose a console-mode run for local diagnostics (same behavior but writes to stdout).
- FR5: Configuration (topic, headers, stabilization wait, retries, backoff) shall be stored in a JSON config file in the install folder and readable by admins.

## 5.3 Non-functional Requirements
- NFR1: Memory footprint target: < 30 MB after startup (trim single-file publish recommended).
- NFR2: Time-to-notify on successful network: <= 30s after stabilization (target end-to-end).
- NFR3: Resilience: honor Windows service restart recovery settings.

---

# 6. How the End User Interacts (quick guide)

- Installation (admin):
  - Publish single-file, trimmed executable.
  - Copy to C:\Program Files\Satark\Satark.exe
  - Run: sc.exe create "Satark" binpath= "C:\Program Files\Satark\Satark.exe" start= auto
- Monitoring:
  - Check Event Viewer -> Windows Logs -> Application -> Source: Satark
  - Use the ntfy mobile app to receive push notifications
- Local diagnostics:
  - Run the EXE from a console with `--console` or `--verbose` flags to see immediate logs.

Example Event Viewer success entry:
- Source: Satark
- Event ID: 1000
- Message: "Boot notification sent to ntfy.sh/my-topic — 200 OK — 2026-01-18T10:00:00Z"

---

# 7. Testing Strategy (table + checklists)

## Overview table

| Test Type | Scenario | Expected Result | Notes |
|---|---|---|---|
| Unit | Mock HttpClient responses | Handles 200 OK and 4xx/5xx gracefully | Use IHttpClientFactory and message handler mocks |
| Integration | Run .exe in Console mode | Notification arrives on phone within 5s | Requires reachable ntfy.sh |
| System | Full system reboot | Service starts and sends alert without user login | Tests startup/service permissions |
| Edge | Boot with network unplugged | Service logs error and retries upon reconnection | Validate backoff and retry count |

## Test checklists
- [ ] Unit tests for stabilization delay, network check, retry logic
- [ ] Integration tests against a controlled ntfy endpoint (staging)
- [ ] System test: configure service autostart and reboot VM
- [ ] Security test: ensure config file permissions are admin-only

---

# 8. Deployment Checklist (actionable)

- [ ] Publish as "Single File" and "Trimmed" to reduce footprint.
- [ ] Copy Satark.exe and appsettings.json to C:\Program Files\Satark\
- [ ] Set config (topic, Title header, retry count)
- [ ] Run PowerShell as Admin:
```powershell
sc.exe create "Satark" binpath= "C:\Program Files\Satark\Satark.exe --service" start= auto
sc.exe description "Satark" "Sends boot completion notifications to ntfy.sh"
sc.exe failure "Satark" reset= 86400 actions= restart/60000
```
- [ ] Configure service recovery in services.msc: "Restart the Service" on first/second failure
- [ ] Verify Event Viewer entries after next reboot

---

# 9. Configuration (example JSON)
Place this file next to the executable as `appsettings.Satark.json` (permission: Administrators only).

```json
{
  "Topic": "my-topic",
  "StabilizationSeconds": 30,
  "Retries": 3,
  "BackoffSeconds": 10,
  "Headers": {
    "Title": "System Alert",
    "Priority": "high"
  },
  "LogSourceName": "Satark"
}
```

---

# 10. Observability & Logging
- All attempts are logged to Application event log with:
  - timestamp, attempt number, result code, response body summary (truncated)
- Use Event Source "Satark" and Event ID conventions:
  - 1000 — Notification Sent (Success)
  - 1001 — Notification Failed (Transient)
  - 1002 — Notification Failed (Permanent)
- Optional: Add telemetry hook (disabled by default) for enterprise environments.

---

# 11. Security & Permissions
- Service runs as LocalSystem by default to guarantee startup visibility — consider least-privilege account if needed.
- Config file must be readable only by Administrators.
- Do not store secrets in plaintext; ntfy requires no auth for public topics, use caution with private topics.

---

# 12. Appendix

## CLI examples
- Run in console mode:
```powershell
C:\Program Files\Satark\Satark.exe --console --config "C:\Program Files\Satark\appsettings.Satark.json"
```

## Troubleshooting quick tips
- No notification received: check Event Viewer for 1001/1002 and confirm outbound HTTP allowed by firewall
- Service fails to start: verify install path and service binary permissions

---

Revision history
- 2026-01-18 — v1.0 — Initial structured & interactive SRS (this document)
