# OrithenApp POC

A Windows 11 desktop WPF application in C# with a portable approach and local storage via SQLite.

## Current status

This first version implements a base POC structure with:

- 3 main screens:
  - Exercise management
  - Create template
  - Template visualization
- An initial data model for:
  - exercises
  - session templates
  - blocks
  - stations
  - station exercises
- A local SQLite database stored in a single file: `orithenapp.db`
- Sample seed data to test the app

## Project structure

- `OrithenApp.sln` — main solution
- `OrithenApp/` — WPF project
  - `App.xaml` / `App.xaml.cs` — application startup
  - `MainWindow.xaml` / `MainWindow.xaml.cs` — navigation between screens
  - `Models/` — domain models
  - `Data/DatabaseService.cs` — SQLite initialization and access
  - `Views/` — the three POC screens

## Requirements

- Windows 11
- A .NET SDK compatible with WPF (recommended .NET 8 SDK for Windows)
- Visual Studio 2022 or a similar environment to open the project

## Local execution

1. Open `OrithenApp.sln` in Visual Studio.
2. Restore NuGet packages.
3. Build and run the application.

## Local persistence

The application uses SQLite with a local file in the executable folder:

- `orithenapp.db`

This allows the POC to remain portable by copying the entire folder to another Windows machine.

## Development notes

The current implementation is a functional base for validating the POC flow. The next step will be to:

- complete the CRUD for exercises,
- save complete templates with blocks and stations,
- and render the template more like a training board.

## Current environment limitation

In this execution environment, it was not possible to complete restore and build operations because the available .NET SDK returned:

- `No .NET SDKs were found.`

For that reason, the project structure is already created, but real compilation should be verified on a machine with the .NET SDK installed.
