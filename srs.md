Software Requirements Specification (SRS)
Project: BootNotifier Windows Service
Version: 1.0
Date: January 18, 2026
1. Introduction
1.1 Purpose
The purpose of this document is to define the functional and non-functional requirements for a Windows Service that monitors system boot events and sends a push notification to a mobile device via the NTFY API.
1.2 Intended Audience
This document is intended for developers, system administrators, and QA engineers involved in the creation and maintenance of the background utility.
1.3 Product Scope
The application will function as a "headless" background worker. It will not have a GUI but will integrate with the Windows Service Control Manager (SCM) and external web APIs.

2. Overall Description
2.1 User Needs
Users require a hands-off method to be alerted immediately when their remote or local workstation finishes the boot sequence and gains network access.
2.2 Assumptions and Dependencies
Internet Connectivity: The system must have an active internet connection to reach the NTFY servers.
Operating System: Optimized for Windows 10/11 and Windows Server 2019+.
Third-Party API: Dependency on ntfy.sh availability.

3. Process Flow
The application follows a linear trigger-based flow upon system startup.
Stage
Process
Description
I
Boot Trigger
Windows SCM executes the binary with LocalSystem privileges.
II
Stabilization
Service waits for 30s to ensure network drivers are fully initialized.
III
Web Request
Asynchronous POST request is sent to https://ntfy.sh/{topic}.
IV
Heartbeat
Service logs "Success" to Event Viewer and enters a low-resource idle loop.


4. System Requirements & Tools
4.1 Development Environment
IDE: Visual Studio 2022 or VS Code.
Framework: .NET 8.0 SDK (Worker Service Template).
Language: C# 12.
Libraries: Microsoft.Extensions.Hosting.WindowsServices.
4.2 Production Environment
Runtime: .NET 8 Runtime (Desktop or Server).
Permissions: Administrative access to install via sc.exe.

5. Specific Requirements
5.1 External API Requirements
Provider: NTFY (Simple HTTP-based pub-sub).
Method: POST.
Headers: Title: System Alert, Priority: High.
5.2 Functional Requirements
FR1: The service shall automatically start within 2 minutes of system power-on.
FR2: The service shall retry the notification up to 3 times if the network is unavailable.

6. How the End User Interacts
As a background service, user interaction is minimal:
Installation: Handled via PowerShell/CMD using sc.exe.
Monitoring: The user checks the Windows Event Viewer (under Application logs) to verify successful notification attempts.
Reception: The user interacts with the NTFY Mobile App to receive the push notification.

7. Testing Strategy
Test Type
Scenario
Expected Result
Unit Test
Mocking HttpClient response.
Ensure code handles 200 OK and 500 Error correctly.
Integration Test
Running the .exe manually in Console mode.
Notification appears on phone within 5 seconds.
System Test
Full system reboot.
Service starts and sends alert without user login.
Edge Case
Booting with Ethernet unplugged.
Service should log an error and retry upon reconnection.


8. Deployment Checklist
[ ] Publish project as "Single File" and "Trimmed" to reduce footprint.
[ ] Copy .exe to C:\Program Files\BootNotifier\.
[ ] Run PowerShell as Admin: sc.exe create "BootNotifier" binpath= "..." start= auto.
[ ] Configure "Recovery" tab in services.msc to "Restart Service" on failure.

