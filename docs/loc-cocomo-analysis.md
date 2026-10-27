# LOC and COCOMO Analysis

## 1. Objective

This analysis estimates the source code size of the legacy ASP.NET Web Forms application and its modern .NET Blazor replacement. It also provides a preliminary software effort estimation using the Basic COCOMO model.

The analysis is intended to support the modernization project by documenting code size, measurement assumptions, and estimation limitations.

## 2. Measurement Methodology

Source files were analyzed using PowerShell scripts. Approximate lines of code (LOC) were calculated by excluding blank lines and lines beginning with common comment markers.

The measurements are not parser-based and may include inline comments, markup, declarations, or generated content.

The following source file types were considered:

- Legacy: `.cs`, `.aspx`, `.Master`, `.ascx`, `.asax`
- Modern: `.cs`, `.razor`
- CSS files were measured separately.

Third-party JavaScript and CSS libraries, source maps, images, fonts, build artifacts, and other non-application assets were excluded from the primary LOC comparison.

Automatically generated `.designer.cs` files were identified and excluded from the adjusted Legacy LOC.

## 3. LOC Measurement Results

| Source Category | Legacy LOC | Modern LOC |
|---|---:|---:|
| C# (excluding designer files) | 1,185 | 140 |
| ASPX / Razor | 467 | 227 |
| Master / ASCX / ASAX | 95 | 0 |
| **Adjusted Application LOC** | **1,747** | **367** |
| Generated C# designer code | 72 | 0 |
| CSS (separately measured) | 238 | 462 |

The original Legacy measurement, including generated designer code, was 1,819 LOC.

The adjusted Legacy measurement is:

1,819 - 72 = **1,747 LOC**

The Modern application currently contains **367 LOC** in the measured C# and Razor files.

## 4. Third-Party and Generated Code

The Legacy application contains third-party libraries such as Bootstrap, jQuery, Popper, Modernizr, and ASP.NET Web Forms JavaScript components.

The Modern application also contains Bootstrap distribution files.

These libraries were excluded to avoid treating externally maintained code as project development effort.

The Legacy application contains 10 `.designer.cs` files with a combined approximate size of 72 LOC. These files were excluded from the adjusted application LOC.

CSS measurements are reported separately because some stylesheets may originate from framework or project templates.

## 5. Basic COCOMO Estimation

The Basic COCOMO model estimates software development effort using:

**Effort (person-months) = a × (KLOC)^b**

For the Organic project category:

- a = 2.4
- b = 1.05
- KLOC = LOC / 1,000

### Legacy Application

KLOC = 1,747 / 1,000 = 1.747

Estimated effort:

**E = 2.4 × (1.747)^1.05 ≈ 4.31 person-months**

### Modern Application (Current State)

KLOC = 367 / 1,000 = 0.367

Estimated effort:

**E = 2.4 × (0.367)^1.05 ≈ 0.84 person-months**

| Application | KLOC | Estimated Effort |
|---|---:|---:|
| Legacy Web Forms | 1.747 | 4.31 person-months |
| Modern Blazor (in progress) | 0.367 | 0.84 person-months |

## 6. Interpretation and Limitations

The results are preliminary and should not be interpreted as actual development costs.

Important limitations include:

- The Modern application is still under development.
- Basic COCOMO assumes a conventional software development context and does not directly model the effort of migrating an existing application.
- The Organic project category is an illustrative assumption rather than an empirically validated classification.
- C#, ASPX, and Razor LOC are combined for a simplified comparison, although markup and executable code are not equivalent.
- Some code may originate from templates or existing implementations.
- CSS is excluded from the primary COCOMO calculation.
- The actual development effort should be compared with recorded team working hours.

## 7. Future Updates

Once the Modern application is complete, the team should:

1. Repeat the LOC measurement.
2. Review generated and template-based code exclusions.
3. Recalculate the COCOMO estimate using the final codebase.
4. Compare theoretical effort estimates with recorded development hours.
5. Document differences between estimated and actual effort.

## 8. Conclusion

The adjusted Legacy application currently contains approximately **1,747 LOC**, while the Modern Blazor implementation contains approximately **367 LOC**.

The preliminary Basic COCOMO estimates are **4.31 person-months** for the Legacy codebase and **0.84 person-months** for the current Modern codebase.

These figures provide an initial size-based comparison, not a definitive measurement of modernization effort. Final conclusions should be drawn after implementation and evaluation are complete.