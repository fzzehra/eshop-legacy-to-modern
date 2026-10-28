# Code Coverage Analysis

## 1. Objective

The objective of this analysis is to evaluate the automated test coverage of the modernized eShop application and identify areas that require additional testing.

The analysis focuses on the `CatalogService` and `CatalogReadService` classes in the ASP.NET Core .NET 10 application.

## 2. Testing Environment

- **Framework:** .NET 10
- **Unit testing framework:** xUnit
- **Database testing:** Entity Framework Core InMemory
- **Coverage collector:** Coverlet (`coverlet.collector`)
- **Coverage report format:** Cobertura XML

The unit tests use isolated in-memory databases, allowing the catalog services to be tested without a running SQL Server instance.

## 3. Test Coverage

Six new unit tests were implemented for `CatalogService`.

| Tested operation | Test scenarios |
|---|---|
| Get catalog brands | Verify that brands are returned in ascending ID order |
| Get catalog types | Verify that types are returned in ascending ID order |
| Create catalog item | Verify ID assignment when the database is empty |
| Create catalog item | Verify ID assignment when existing items are present |
| Update catalog item | Verify that changes are persisted |
| Remove catalog item | Verify that the item is removed |

Five existing `CatalogReadService` tests were also executed.

All 11 selected tests passed successfully.

## 4. Coverage Results

| Metric | Before | After |
|---|---:|---:|
| Overall Line Coverage | 30.59% | 37.39% |
| Overall Branch Coverage | 3.94% | 3.94% |
| CatalogService Line Coverage | 0% | 100% |

Overall line coverage increased by **6.80 percentage points**.

The `CatalogService` class achieved **100% line coverage** after the new unit tests were introduced.

Branch coverage remained unchanged at 3.94%, indicating that further testing is required for conditional logic in other parts of the application.

## 5. Test Execution

The following command was used to execute the catalog service tests and collect coverage data:

```powershell
dotnet test .\tests\Tests\Tests.csproj --filter "FullyQualifiedName~CatalogServiceTests|FullyQualifiedName~CatalogReadServiceTests" --collect:"XPlat Code Coverage"
```

**Execution results:**

- Total tests: 11
- Passed: 11
- Failed: 0
- Skipped: 0

The coverage report was generated in Cobertura XML format under the `tests/Tests/TestResults` directory.

## 6. Limitations and Future Improvements

Although `CatalogService` achieved 100% line coverage, this does not guarantee that every possible input or failure scenario has been tested.

Further improvements may include:

- Testing invalid and missing catalog items.
- Testing database error handling.
- Testing concurrent catalog item creation, particularly the current maximum-ID-based assignment approach.
- Adding component tests for the Blazor catalog Create, Edit, and Delete pages.
- Increasing branch coverage across the application.
- Running integration and end-to-end tests against the deployed modern application.

The reported overall coverage values apply to the measured .NET assemblies and should not be interpreted as coverage of the entire repository, including the legacy application and browser-based end-to-end tests.

## 7. Conclusion

The introduction of six new `CatalogService` unit tests increased overall line coverage from **30.59% to 37.39%** and improved `CatalogService` line coverage from **0% to 100%**.

All 11 catalog service tests passed. These results demonstrate improved automated verification of the catalog service layer and establish a baseline for further testing.