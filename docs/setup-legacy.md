# Legacy Application Setup Guide

This document explains how to set up and run the legacy ASP.NET Web Forms application used as the baseline of the modernization project.

## 1. Prerequisites

The legacy application was tested on Windows.

Required tools:

- Windows
- Visual Studio 2022
- ASP.NET and web development workload
- .NET Framework development tools
- Entity Framework 6 Tools
- SQL Server LocalDB
- Git

In Visual Studio Installer, make sure the **ASP.NET and web development** workload is installed.

## 2. Source Application

The legacy application is based on Microsoft's eShopModernizing repository:

https://github.com/dotnet-architecture/eShopModernizing

The Web Forms solution used in this project is:

`eShopLegacyWebForms.sln`

The application is used as the legacy baseline and its existing functionality should not be modified during the initial setup.

## 3. Running with Mock Data

Open `eShopLegacyWebForms.sln` in Visual Studio 2022.

Open `Web.config` and check the following setting:

```xml
<add key="UseMockData" value="true" />
```

When `UseMockData` is set to `true`, the application uses mock data instead of LocalDB.

Run the application using **IIS Express**.

If the setup is successful, the legacy product catalog should be displayed in the browser.

## 4. Known Setup Issue: Missing Roslyn Compiler

During the first run, the following error was encountered:

```text
System.IO.DirectoryNotFoundException:
...\eShopLegacyWebForms\bin\roslyn\csc.exe
```

The required Roslyn compiler files were available in the NuGet package directory, but the `bin\roslyn` directory had not been generated.

To resolve the issue:

1. Create a `roslyn` folder under:

   `eShopLegacyWebForms\bin\`

2. Locate the `Roslyn45` folder in the installed `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` package.

3. Copy the contents of `Roslyn45` into:

   `eShopLegacyWebForms\bin\roslyn\`

4. Run the application again using IIS Express.

After copying the compiler files, the application started successfully.

### NuGet Dependency Warning

While troubleshooting, reinstalling the NuGet packages was attempted. NuGet reported a dependency conflict between:

- `AspNet.ScriptManager.jQuery 3.3.1`
- `jQuery 3.5.0`

Since the purpose of this stage is to preserve the legacy baseline, the package versions were not changed.

Visual Studio also reported security warnings for some legacy dependencies. These packages were not upgraded during the baseline setup.

## 5. Running with LocalDB

The application was also tested using SQL Server LocalDB.

To verify that LocalDB is installed, open Command Prompt and run:

```cmd
sqllocaldb info
```

The following instance should be available:

```text
MSSQLLocalDB
```

The database connection is configured in `Web.config`:

```xml
<connectionStrings>
  <add name="CatalogDBContext"
       connectionString="Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Microsoft.eShopOnContainers.Services.CatalogDb; Integrated Security=True; MultipleActiveResultSets=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

To run the application using LocalDB, change:

```xml
<add key="UseMockData" value="true" />
```

to:

```xml
<add key="UseMockData" value="false" />
```

Save `Web.config` and run the application again using IIS Express.

## 6. Legacy Functionality Verification

The following product management functions were manually verified using LocalDB:

1. Product List
2. Product Details
3. Product Create
4. Product Edit
5. Product Delete

A temporary test product was created, edited and deleted to verify the CRUD behavior without leaving test data in the database.

## 7. Legacy Screenshots

Screenshots of the verified legacy functionality are stored in `docs/legacy-screens/`:

- [Product List](legacy-screens/01-product-list.png)
- [Product Details](legacy-screens/02-product-details.png)
- [Product Create](legacy-screens/03-product-create.png)
- [Product Edit](legacy-screens/04-product-edit.png)
- [Product Delete](legacy-screens/05-product-delete.png)

These screenshots document the existing behavior of the legacy application before functional modernization begins.