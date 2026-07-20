# OrithenApp POC

A Windows 11 desktop WPF application in C# with local SQLite persistence and a simple three-screen flow.

## Current status

The current prototype includes:

- 3 main screens:
  - Exercise management
  - Create template
  - Visualize templates
- A local SQLite database stored in a single file: `orithenapp.db`
- Exercise entries grouped by category in the management view
- A template creation form with:
  - a template-name field
  - single-select fields for Programa and Metodologia
  - multi-select checkboxes for Escalfament
  - dynamic blocks with exercise checkboxes
- A template viewer that loads saved templates from the database and shows the details of the selected one

## Project structure

- `OrithenApp.sln` — solution entry point
- `OrithenApp/` — WPF project
  - `App.xaml` / `App.xaml.cs` — application startup
  - `MainWindow.xaml` / `MainWindow.xaml.cs` — navigation between screens
  - `Models/` — domain models for exercises and templates
  - `Data/DatabaseService.cs` — SQLite initialization and connection helpers
  - `Views/` — the three app screens
- `.gitignore` — excludes build output and temporary files from Git

## Requirements

- Windows 11
- A .NET SDK compatible with WPF (recommended .NET 8 SDK for Windows)
- Visual Studio 2022 or a similar IDE able to open a WPF solution

## Run locally

### Using Visual Studio

1. Open `OrithenApp.sln` in Visual Studio.
2. Restore NuGet packages.
3. Build the solution.
4. Press F5 to launch the app.

### Using the command line

From the repository root:

```powershell
dotnet build OrithenApp.sln -c Debug
Start-Process .\OrithenApp\bin\Debug\net8.0-windows\OrithenApp.exe
```

## Build status

The project has been verified in the current environment with:

```powershell
dotnet build OrithenApp.sln -c Debug
```

Result: build completed successfully.

## Local persistence

The app uses SQLite with a local file generated in the output folder:

- `orithenapp.db`

This keeps the POC portable enough to copy the whole folder to another Windows machine.

## Current functionality

The app currently supports:

- adding exercise entries from the management page
- grouping stored data by category in the management view
- creating templates that store the selected program, warmup, methodology and block exercises
- viewing all saved templates and opening one to inspect its stored details

## Repository hygiene

Generated files are excluded from Git via `.gitignore`, including:

- `OrithenApp/bin/`
- `OrithenApp/obj/`
- temporary Visual Studio and build artifacts
