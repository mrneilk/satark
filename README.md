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
## Process Flow
<p align="center">
<img alt=".NET logo" src="https://github.com/mrneilk/satark/blob/main/Process%20Flow%20V1.png" width="160" length="500"/>
</p>
## Quick Start - Not Ready yet, to be updated when the program testing completes

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
   This will be updated once a package is released

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

//

## Roadmap & Logs // Logs Moved to logfile.txt - to be updated



## Contributing

Contributions welcome — open issues and PRs. Consider adding a CONTRIBUTING.md for contribution guidelines and code style.

## License
```
MIT © mrneilk
```

---

Maintained by mrneilk
