# Satark

[![Built with .NET](https://img.shields.io/badge/Built%20with-.NET-512BD4?style=flat-square)](https://dotnet.microsoft.com)
[![Status](https://img.shields.io/badge/status-alpha-yellow?style=flat-square)]()

A lightweight Windows backend service that notifies when a computer powers on. Designed for simple deployment and integration with notification backends (initially Google Firebase).

---

<p align="center">
  <img alt=".NET logo" src="https://raw.githubusercontent.com/dotnet/brand/29878855347e055ff15675471f7043fda3e92cea/logo/dotnet-logo.svg" width="160"/>
</p>

## Table of Contents

- [Features](#features)
- [Demo / Screenshot](#demo--screenshot)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Roadmap & Logs](#roadmap--logs)
- [Contributing](#contributing)
- [License](#license)

## Features

- Runs as a Windows backend/service process
- Sends notifications when a machine powers on
- Initial integration with Google Firebase (expandable to other APIs)
- Small footprint and easy configuration

## Demo / Screenshot
- ## To be Updated

## Quick Start

1. Clone the repo:

   ```bash
   git clone https://github.com/mrneilk/satark.git
   cd satark
   ```

2. Build (example for .NET):

   ```bash
   dotnet build
   ```

3. Configure notifications (see [Configuration](#configuration)).

4. Install and run as a Windows service (replace with your service installer or sc.exe steps):

   ```powershell
   # Example (replace ServiceName and paths):
   sc create SatarkService binPath= "C:\path\to\satark.exe" start= auto
   sc start SatarkService
   ```

(Replace commands above with actual build/run steps if different.) - TO BE UPDATED

## Configuration - to be update #WIP

- Firebase: Provide your Firebase credentials (service account JSON or environment variables).
- Notification endpoint: Configure the target API or webhook the service should call when a power-on event is detected.
- Logging: Configure log level and retention in appsettings or environment variables.

Example (appsettings.json snippet):

```json
{
  "Firebase": {
    "ProjectId": "your-project-id",
    "CredentialsPath": "./firebase-service-account.json"
  },
  "Notification": {
    "Endpoint": "https://example.com/notify"
  }
}
```

## Roadmap & Logs

- 14-01-2026: Issue updated with possible solution resource - TBC
- 12-01-2026: Added issue to the repository
- 09-01-2026: Test Service run unsuccessful/Need to understand more about win services in depth.
- 08-01-2026: Created Windows Service Template from Winos Service Resources
- 05-01-2026: Update Readme with AI
- 2024-01-01: Planned: initial Firebase integration.
- Future: Add support for additional notification APIs, improve reliability, add CI and tests.

## Contributing

Contributions welcome — open issues and PRs. Consider adding a CONTRIBUTING.md for contribution guidelines and code style.

## License - NEEDS UPDATE

Add a LICENSE file to clarify terms. Example (if you choose MIT):

```
MIT © mrneilk
```

---

Maintained by mrneilk
