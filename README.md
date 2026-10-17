# From Legacy to Modern: eShop Web Forms → Blazor

Engineering Design II — Project 01 (Software Modernization)

This project analyzes Microsoft's legacy eShopLegacyWebForms application
(ASP.NET Web Forms, .NET Framework) and migrates it incrementally to
Blazor on .NET 10, preserving existing behavior with tests and comparing
the legacy and modern versions with measurable metrics.

The application is a product catalog with five operations:
**List, Details, Create, Edit, Delete**.

Based on: https://github.com/dotnet-architecture/eShopModernizing

## Repository Structure

```
.
├── legacy/              # Original legacy Web Forms application (MIT)
├── modern/              # Modernized Blazor application (.NET 10)
├── tests/               # Characterization and regression tests
│   └── Tests/           # xUnit test project
├── benchmarks/          # Performance and reliability tests
├── docs/                # Analysis, reports and presentations
│   ├── legacy-screens/  # Screenshots of the five legacy screens
│   ├── behavior.md      # Expected behavior of each screen
│   ├── seed-data.md     # Initial database data
│   ├── setup-legacy.md  # Legacy app setup guide
│   └── setup-modern.md  # Modern app setup guide
└── .github/workflows/   # GitHub Actions CI
```

## Setup Guides

- [Legacy application (Windows, Visual Studio)](docs/setup-legacy.md)
- [Modern application (.NET 10, VS Code)](docs/setup-modern.md)

## Running the Tests

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet test tests/Tests/Tests.csproj
```

## Continuous Integration

On every push and pull request, GitHub Actions builds the project and runs
the tests: [.github/workflows/ci.yml](.github/workflows/ci.yml).

## Workflow

- Each team member works on a separate branch and merges into `main`
  through a pull request.
- Board cards move to **In progress** when work starts and to **Done**
  when it is finished.
