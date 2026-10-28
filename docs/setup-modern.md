# Modern Application Setup Guide

This document explains how to set up and run the new Blazor application that is replacing the legacy ASP.NET Web Forms app.

## 1. Prerequisites

The modern application was built and tested on macOS, but it also runs on Windows and Linux.

Required tools:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio Code](https://code.visualstudio.com/) with the **C# Dev Kit** extension
- Git

Verify the SDK is installed:

```bash
dotnet --version
# should print 10.0.x
```

## 2. Project Location

The Blazor project lives in:

`modern/EShop.Modern.csproj`

It is a standard **Blazor Web App** (server interactivity), scaffolded with:

```bash
dotnet new blazor -n EShop.Modern -o modern
```

## 3. Running the Application

From the repository root:

```bash
cd modern
dotnet run
```

By default the app listens on `http://localhost:5232` (and `https://localhost:7055` if using the `https` launch profile), and opens automatically in your browser.

Pages available so far:

- `/` — Home
- `/counter` — sample counter page from the template
- `/weather` — sample data page from the template
- `/catalog` — placeholder page for the migrated product catalog

## 4. Building and Testing

Build only:

```bash
dotnet build modern/EShop.Modern.csproj
```

CI (`.github/workflows/ci.yml`) automatically builds every `.csproj` under `modern/` on each push and pull request, in addition to running the test suite in `tests/`.

## 5. Opening in VS Code

```bash
code modern
```

With the C# Dev Kit extension installed, VS Code will offer to restore dependencies and provide debugging (F5) support.

## Docker ile çalıştırma (uygulama + SQL Server)

Mac'te SQL Server yalnızca Docker ile çalışır. Docker Desktop açıkken, reponun ana klasöründe:

```bash
docker compose up --build
```

- Uygulama: http://localhost:8080
- `db/init.sql`, legacy şemasını ve 12 başlangıç ürününü oluşturur (bkz. [seed-data.md](seed-data.md)).
- Apple Silicon Mac'te Docker Desktop → Settings → General → **Use Rosetta** açık olmalı.
- Durdurmak için: `docker compose down` (veriyi de silmek için `docker compose down -v`).
