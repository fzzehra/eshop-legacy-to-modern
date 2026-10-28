
# CodeQL Static Code Analysis Report

## 1. Objective

This report evaluates the security and code quality of the legacy ASP.NET Web Forms application and the modern Blazor application using GitHub CodeQL.

The objective is to identify potential security vulnerabilities, maintainability issues, and code quality improvements during the modernization process.

## 2. Analysis Environment

- Tool: GitHub CodeQL
- Platform: GitHub Actions
- Operating system: Windows (windows-latest)
- Language: C#
- Analysis mode: Buildless (build-mode: none)
- Query suite: security-and-quality
- Analysis date: October 28, 2026
- Branch: main
- Commit: 545f01c

The analysis was executed automatically through the `.github/workflows/codeql.yml` workflow.

Both the CodeQL workflow and the existing CI workflow completed successfully.

## 3. Analysis Results

CodeQL reported 22 open findings across the repository.

| Application | Open Findings |
|---|---:|
| Legacy ASP.NET Web Forms | 21 |
| Modern Blazor | 1 |
| Total | 22 |

These findings include security-related alerts and code quality recommendations.

## 4. Legacy Application Findings

### 4.1 Missing Clickjacking Protection

- Severity: High
- Rule ID: cs/web/missing-x-frame-options
- CWE: CWE-451
- File: legacy/eShopLegacyWebFormsSolution/src/eShopLegacyWebForms/Web.config

CodeQL detected missing clickjacking protection in the legacy application's configuration.

Without appropriate framing restrictions, an application may be embedded within a malicious website, potentially causing users to interact with unintended interface elements.

Recommended mitigations include:

- X-Frame-Options: SAMEORIGIN
- Content-Security-Policy: frame-ancestors 'self'

The appropriate configuration should be selected based on the application's legitimate framing requirements.

This is a static analysis finding and has not been independently verified through exploitation testing.

### 4.2 Other Code Quality Findings

Additional findings include:

- Useless assignment to local variable
- Potential issues involving System.IO.Path.Combine
- Inefficient use of ContainsKey

These findings suggest opportunities to improve code clarity, reliability, and maintainability.

Individual findings should be reviewed before changes are made.

## 5. Modern Application Findings

CodeQL reported one low-priority code quality finding in the modern application.

- Finding: Missed 'readonly' opportunity
- Severity: Note
- Rule ID: cs/missed-readonly-modifier
- File: modern/Components/Pages/CatalogCreate.razor
- Line: 103

The following field was identified:

```csharp
private CatalogItem _item = new()
```

CodeQL suggests that the field could be declared as readonly:

```csharp
private readonly CatalogItem _item = new()
```

This recommendation improves code maintainability by preventing reassignment of the field after initialization.

It does not represent a confirmed security vulnerability.

## 6. Legacy vs. Modern Comparison

| Metric | Legacy | Modern |
|---|---:|---:|
| Total CodeQL findings | 21 | 1 |
| Confirmed high-severity CodeQL alerts | At least 1 | 0 |
| Code quality recommendations | Yes | Yes |

The modern application currently has fewer reported findings than the legacy application.

However, these results do not establish that the modern application is proportionally more secure. Differences in codebase size, implemented functionality, and analysis coverage must be considered.

## 7. Limitations

- The analysis used CodeQL's buildless mode.
- Some issues may not be detected without a full build.
- Static analysis findings do not necessarily indicate exploitable vulnerabilities.
- The modern application is still under development.
- Results reflect the repository at commit 545f01c.

## 8. Conclusion

The CodeQL analysis successfully identified security and maintainability issues in the legacy and modern applications.

The most significant identified issue is missing clickjacking protection in the legacy application's Web.config file.

The modern application currently has one low-priority maintainability recommendation.

The findings provide a baseline for tracking security and code quality improvements during modernization.
