# Large Codebase Research

## 1. Objective

This research evaluates large open-source C#/.NET codebases based on source code size, architecture, modularity, and maintainability.

## 2. Selected Projects

Two open-source e-commerce applications were examined:

- **nopCommerce:** https://github.com/nopSolutions/nopCommerce
- **Microsoft eShop:** https://github.com/dotnet/eShop

Both projects use the .NET ecosystem and provide relevant examples for understanding large-scale application architecture.

## 3. Source Code Size Comparison

| Metric | nopCommerce | Microsoft eShop |
|---|---:|---:|
| Total C# files | 3,662 | 547 |
| C# files excluding tests | 3,504 | 498 |
| Total approximate LOC | 232,539 | 20,690 |
| Approximate LOC excluding tests | 213,914 | 17,761 |
| KLOC excluding tests | 213.91 | 17.76 |
| Git commit | `2213058` | `913acea` |

nopCommerce contains approximately 12 times more non-test C# code than Microsoft eShop.

## 4. nopCommerce Module Analysis

| Module | Files | Approximate LOC |
|---|---:|---:|
| Nop.Web | 1,149 | 76,178 |
| Nop.Services | 586 | 53,288 |
| Plugins | 943 | 52,617 |
| Nop.Web.Framework | 250 | 16,870 |
| Nop.Data | 206 | 7,500 |
| Nop.Core | 369 | 7,364 |
| Other | 1 | 97 |
| **Total** | **3,504** | **213,914** |

The largest module is Nop.Web, followed by Nop.Services and Plugins.

## 5. Architectural Characteristics

nopCommerce follows a layered architecture with responsibilities distributed across several components:

- **Nop.Core:** Core domain objects and foundational components.
- **Nop.Data:** Data access and persistence.
- **Nop.Services:** Business logic and application services.
- **Nop.Web:** Web application and administration functionality.
- **Nop.Web.Framework:** Shared web infrastructure.
- **Plugins:** Extensible application functionality.

## 6. Maintainability Considerations

Large codebases introduce several engineering challenges:

- Changes may affect multiple modules and layers.
- Regression testing becomes increasingly important.
- Plugin compatibility must be maintained during upgrades.
- New developers require time to understand module responsibilities and dependencies.
- Modular architecture helps separate responsibilities and organize development.

These are architectural risk considerations rather than measured defect rates.

## 7. Measurement Methodology

The repositories were cloned using `git clone --depth 1`.

The analysis included `.cs` files and excluded `bin`, `obj`, and `.git` directories.

Test directories were excluded based on directory naming patterns.

Approximate LOC was calculated by excluding blank lines and lines beginning with common comment markers (`//`, `*`, `/*`, `*/`).

**Limitations:** This method does not fully parse C# syntax, does not reliably eliminate all multiline or inline comments, and does not include other programming languages. Therefore, the reported values are approximate C# LOC measurements.

## 8. Conclusion

nopCommerce was selected as the primary large-codebase research candidate because of its significantly larger C# codebase, layered architecture, and extensible plugin system.

Microsoft eShop serves as a smaller, modern .NET comparison project.

The measured code sizes can support further software engineering analysis, but any COCOMO estimation should explicitly define its scope, assumptions, and estimation model.

## References

- https://github.com/nopSolutions/nopCommerce
- https://github.com/dotnet/eShop
- https://docs.nopcommerce.com/en/developer/tutorials/architecture-of-nopCommerce.html
- https://docs.nopcommerce.com/en/developer/tutorials/source-code-organization.html