# Legacy System.Web Dependency Inventory

## 1. Purpose

This document identifies System.Web-related dependencies in the legacy eShop Web Forms application and describes possible migration approaches for Blazor and ASP.NET Core.

The inventory covers page lifecycle mechanisms, server controls, routing, session management, application startup, static resource management, and file system access.

## 2. Dependency Overview

| Dependency | Legacy Location | Modernization Approach |
|---|---|---|
| `System.Web.UI.Page` | `Default.aspx.cs`, `About.aspx.cs`, `Contact.aspx.cs`, `Catalog/Create.aspx.cs`, `Catalog/Edit.aspx.cs`, `Catalog/Details.aspx.cs`, `Catalog/Delete.aspx.cs` | Replace Web Forms pages with Blazor Razor components |
| `System.Web.UI.MasterPage` | `Site.Master.cs`, `Site.Mobile.Master.cs` | Use `MainLayout.razor` |
| `Page_Load` | Catalog pages and master page | Use Blazor lifecycle methods |
| `Page.IsPostBack` | `Catalog/Edit.aspx.cs` | Use component state and event handling |
| `Page.RouteData.Values` | Catalog pages and `Default.aspx.cs` | Use route parameters with `[Parameter]` |
| `DataBind()` | Catalog pages | Use Razor expressions and component rendering |
| `ModelState.IsValid` | `Catalog/Create.aspx.cs`, `Catalog/Edit.aspx.cs` | Use `EditForm` and Blazor validation |
| `Response.Redirect()` | Create, Edit, Delete code-behind files | Use `NavigationManager.NavigateTo()` |
| `HttpContext.Current.Session` | `Global.asax.cs`, `Site.Master.cs` | Redesign session/state handling for the selected Blazor hosting model |
| `HttpContext.Current.Request` | `Global.asax.cs` | Use ASP.NET Core request context in middleware |
| `System.Web.Routing` | `App_Start/RouteConfig.cs` | Use ASP.NET Core and Blazor routing |
| `System.Web.Optimization` | `App_Start/BundleConfig.cs` | Use modern static asset management |
| `System.Web.UI.ScriptManager` | `Site.Master`, `BundleConfig.cs` | Replace legacy script management with supported JavaScript loading |
| `System.Web.Hosting.HostingEnvironment` | `Models/Infrastructure/CatalogDBInitializer.cs` | Use `IWebHostEnvironment` or `IHostEnvironment` |
| `HttpApplication` | `Global.asax.cs` | Move startup and request handling to ASP.NET Core |
| `Autofac.Integration.Web` | `Global.asax.cs` | Use ASP.NET Core dependency injection or a compatible Autofac integration |

## 3. Web Forms Pages and Server Controls

### Affected Files

- `Default.aspx` and `Default.aspx.cs`
- `Catalog/Create.aspx` and `Catalog/Create.aspx.cs`
- `Catalog/Edit.aspx` and `Catalog/Edit.aspx.cs`
- `Catalog/Details.aspx` and `Catalog/Details.aspx.cs`
- `Catalog/Delete.aspx` and `Catalog/Delete.aspx.cs`

### Identified Dependencies

The legacy catalog pages inherit from `System.Web.UI.Page` and rely on Web Forms controls and code-behind event handlers.

The application uses `asp:ListView`, `asp:TextBox`, `asp:DropDownList`, `asp:Label`, `asp:Image`, `asp:Button`, `asp:HyperLink`, `asp:RequiredFieldValidator`, and `asp:RangeValidator`.

Page data is loaded through `Page_Load`, bound using `DataBind()`, and updated through server-side button events.

### Migration Approach

Replace `.aspx` pages with `.razor` components. Render collections using Razor, use Blazor input components for forms, and implement event handlers with Blazor callbacks.

Replace legacy form validation with an appropriate model-based validation approach.

### About and Contact Page Dependencies

#### Affected files:
- `About.aspx.cs`
- `Contact.aspx.cs`

Both pages inherit from `System.Web.UI.Page` and use the Web Forms `Page_Load` lifecycle event.

The pages use `log4net` to record informational messages when they are loaded.

Migration approach: Replace both Web Forms pages with Blazor Razor components.

If equivalent initialization behavior is required, use appropriate Blazor lifecycle methods such as `OnInitializedAsync()`.

Use ASP.NET Core's `ILogger<T>` abstraction for logging, adapting the existing logging configuration as needed.

### Generated Web Forms Designer Files

#### Affected files include:
- `Default.aspx.designer.cs`
- `Catalog/Create.aspx.designer.cs`
- `Catalog/Edit.aspx.designer.cs`
- `Catalog/Delete.aspx.designer.cs`
- `Catalog/Details.aspx.designer.cs`
- `Site.Master.designer.cs`
- `Site.Mobile.Master.designer.cs`
- `ViewSwitcher.ascx.designer.cs`

These generated files declare server-side controls associated with ASP.NET Web Forms pages, master pages, and user controls.

They are part of the legacy Web Forms control model and may reference types from `System.Web.UI` and `System.Web.UI.WebControls`.

**Migration approach:** Replace the Web Forms controls with Blazor Razor components and standard HTML elements. Generated Web Forms designer files are not required in the modern Blazor application.

## 4. Routing Dependencies

### Affected Files

- `App_Start/RouteConfig.cs`
- `Default.aspx.cs`
- `Catalog/Edit.aspx.cs`
- `Catalog/Details.aspx.cs`
- `Catalog/Delete.aspx.cs`

### Identified Dependencies

The application uses `System.Web.Routing.RouteCollection` and `MapPageRoute()` to define URL patterns.

The following routes are registered:

| Legacy Route | Target Page |
|---|---|
| `/Default` | `Default.aspx` |
| `/Default/index/{index}/size/{size}` | `Default.aspx` |
| `/Catalog/Create` | `Catalog/Create.aspx` |
| `/Catalog/Edit/{id}` | `Catalog/Edit.aspx` |
| `/Catalog/Details/{id}` | `Catalog/Details.aspx` |
| `/Catalog/Delete/{id}` | `Catalog/Delete.aspx` |

Page code-behind files use `Page.RouteData.Values` to retrieve route parameters.

### Migration Approach

Use Blazor's `@page` directive and component parameters. Replace legacy URL generation with standard links or `NavigationManager`.

Preserve existing routes where backward compatibility is required.

## 5. Session Management Dependencies

### Affected Files

- `Global.asax.cs`
- `Site.Master.cs`
- `Site.Master`

### Identified Dependencies

The `Session_Start` method stores the following values:

- `MachineName`
- `SessionStartTime`

These values are accessed through `HttpContext.Current.Session` and displayed in the footer using `SessionInfoLabel`.

### Migration Approach

Remove direct usage of `System.Web.HttpContext.Current`.

Use an appropriate ASP.NET Core session or application state service based on the selected Blazor hosting model.

If the footer continues to display session information, provide it through a dedicated service or component.

## 6. Application Startup and Request Processing

### Affected File

`Global.asax.cs`

### Identified Dependencies

The `Global` class inherits from `HttpApplication`.

The `Application_Start` method registers routes, bundles, dependency injection configuration, and database initialization.

The `Application_BeginRequest` method prepares logging context, including an activity identifier and request information.

The `WebRequestInfo` class accesses the current request URL and user agent through `HttpContext.Current.Request`.

### Migration Approach

Move application startup configuration into ASP.NET Core startup code, typically `Program.cs`.

Use middleware for request-level processing and modern logging mechanisms for request diagnostics.

Replace the Web Forms-specific Autofac container integration with ASP.NET Core dependency injection or a compatible Autofac integration.

## 7. CSS and JavaScript Bundling

### Affected Files

- `App_Start/BundleConfig.cs`
- `Site.Master`

### Identified Dependencies

The legacy application uses `System.Web.Optimization` for script and style bundles.

The registered bundles include:

- Web Forms JavaScript resources
- Microsoft AJAX resources
- Modernizr
- Bootstrap and application CSS files

`ScriptManager` is used to manage script references and resource mappings.

### Migration Approach

Move reusable static resources into the modern application's static asset structure.

Review existing CSS files for compatibility with the new layout.

Remove Web Forms-specific JavaScript resources unless an actual dependency remains.

Use Blazor-compatible JavaScript loading and JavaScript interop only where necessary.

## 8. File System and Hosting Dependencies

### Affected File

`Models/Infrastructure/CatalogDBInitializer.cs`

### Identified Dependencies

The initializer uses `System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath` to locate application files.

This occurs when loading:

- `Setup/CatalogTypes.csv`
- `Setup/CatalogBrands.csv`
- `Setup/CatalogItems.csv`
- Product picture files and `Setup/CatalogItems.zip`

### Migration Approach

Replace `HostingEnvironment.ApplicationPhysicalPath` with a suitable path provided by `IHostEnvironment` or `IWebHostEnvironment`.

Use `ContentRootPath` for application content and `WebRootPath` for public static assets when appropriate.

Review file deletion and ZIP extraction behavior before migrating the picture initialization process.

## 9. Mobile View and Friendly URLs Dependencies

### Affected File

`ViewSwitcher.ascx.cs`

### Identified Dependencies

The legacy application uses `System.Web.UI.UserControl` to implement a reusable view-switching control.

`WebFormsFriendlyUrlResolver.IsMobileView()` and `HttpContextWrapper` are used to determine whether the current request uses the mobile or desktop view.

The control accesses `RouteTable.Routes` to locate the Friendly URLs view-switching route and uses `GetRouteUrl()` to generate a navigation URL.

`HttpUtility.UrlEncode()` and `Request.RawUrl` are used to preserve the current page URL when switching views.

If the view-switching route is unavailable, the control is hidden.

### Migration Approach

Replace the Web Forms user control with a Blazor component only if explicit mobile/desktop switching is still required.

Prefer responsive CSS for adapting the interface to different screen sizes.

If manual view switching is retained, use Blazor navigation and an appropriate state management mechanism instead of Web Forms Friendly URLs and `HttpContextWrapper`.

### Mobile Master Page

#### Affected file: `Site.Mobile.Master.cs`

The legacy mobile master page inherits from `System.Web.UI.MasterPage`.

Its `Page_Load` handler is empty and does not implement additional initialization logic.

During migration, the mobile master page can be replaced with a responsive Blazor layout. A separate mobile layout should only be introduced if the application requires distinct mobile-specific behavior.

The `Site.Mobile.Master` markup defines three Web Forms content placeholders: `HeadContent`, `FeaturedContent`, and `MainContent`.

It also registers and renders the `ViewSwitcher.ascx` user control through the `friendlyUrls:ViewSwitcher` tag.

These placeholders and the user control are tied to the Web Forms rendering model. During migration, the content regions can be redesigned using Blazor layouts and components, while the legacy view-switching mechanism can be replaced with responsive CSS or a dedicated Blazor component if required.

## 10. Legacy ASP.NET Configuration Dependencies

### Affected Files

- `Web.config`
- `Web.Debug.config` and `Web.Release.config` (configuration transforms; both reviewed)

### Identified Dependencies

The legacy application uses `Web.config` to configure the ASP.NET Web Forms runtime, session management, HTTP modules, application settings, and database connectivity.

The configuration includes:

- `system.web` settings for compilation targeting .NET Framework 4.7.2.
- `httpRuntime` targeting .NET Framework 4.6.1 with `requestValidationMode="2.0"`.
- In-process session storage through `sessionState mode="InProc"`.
- Registration of Web Forms optimization controls and namespaces.
- Autofac Web Forms integration modules for dependency injection and container disposal.
- Application Insights and telemetry correlation HTTP modules.
- An asynchronous session state module configured under `system.webServer`.
- Application settings for `UseMockData` and `UseCustomizationData`.
- Entity Framework 6 and SQL Server LocalDB configuration.

### Debug Configuration Transform

The `Web.Debug.config` file uses the legacy XML Document Transform (XDT) format.

The file contains only commented configuration examples and an empty `system.web` section. No active configuration transformations were identified.

Therefore, this file does not currently introduce additional active `System.Web` dependencies. Its necessity should be reassessed during migration to ASP.NET Core environment-specific configuration.

### Release Configuration Transform

The `Web.Release.config` file uses the legacy XML Document Transform (XDT) format.

Unlike the Debug configuration transform, this file contains one active transformation:

`<compilation xdt:Transform="RemoveAttributes(debug)" />`

This transformation removes the `debug` attribute from the `system.web/compilation` element during the Release configuration transformation process.

The remaining configuration examples are commented out and do not introduce additional active dependencies.

During migration, environment-specific settings should be managed using ASP.NET Core configuration providers and appropriate build or deployment settings. The legacy XDT transformation should not be copied directly into the Blazor application.

### Application Insights Configuration

#### Affected file: `ApplicationInsights.config`

The legacy application uses Application Insights for request tracking, exception monitoring, dependency tracking, and performance telemetry.

The configuration contains seven `System.Web` handler references under the `RequestTrackingTelemetryModule` handler list:

- `System.Web.StaticFileHandler`
- `System.Web.Handlers.AssemblyResourceLoader`
- `System.Web.Optimization.BundleHandler`
- `System.Web.Script.Services.ScriptHandlerFactory`
- `System.Web.Handlers.TraceHandler`
- `System.Web.Services.Discovery.DiscoveryRequestHandler`
- `System.Web.HttpDebugHandler`

These entries are handler names used by the legacy telemetry configuration rather than direct application code calls.

The configuration also includes ASP.NET-specific Application Insights modules such as `RequestTrackingTelemetryModule`, `ExceptionTrackingTelemetryModule`, and `AspNetDiagnosticTelemetryModule`.

Migration approach: Replace the legacy ASP.NET Application Insights integration with supported ASP.NET Core Application Insights or OpenTelemetry instrumentation. Review request, exception, dependency, and performance monitoring requirements, and remove obsolete `System.Web` handler configuration.

### Legacy Project References

#### Affected file: `eShopLegacyWebForms.csproj`

The legacy project targets .NET Framework 4.7.2 and uses the classic ASP.NET Web Application project format.

The project explicitly references the following seven `System.Web` assemblies:

- `System.Web`
- `System.Web.Extensions`
- `System.Web.Optimization`
- `System.Web.Services`
- `System.Web.DynamicData`
- `System.Web.Entity`
- `System.Web.ApplicationServices`

The project also references legacy ASP.NET-related packages, including Web Forms ScriptManager components, Autofac Web Forms integration, ASP.NET Friendly URLs, session state modules, and Application Insights Web integration.

The project file includes Web Forms pages, master pages, user controls, code-behind files, and generated designer files.

Migration approach: Create or use a modern SDK-style ASP.NET Core/Blazor project rather than directly transferring the legacy project references.

Remove `System.Web` assembly references and replace required functionality with supported ASP.NET Core and Blazor APIs.

Review third-party dependencies individually and retain only packages compatible with the modern application.

Replace Web Forms-specific project items and generated designer files with Razor components and modern project conventions.

### Migration Approach

Move application configuration to ASP.NET Core configuration providers such as `appsettings.json` and environment variables.

Replace legacy ASP.NET HTTP modules with supported ASP.NET Core middleware and services.

Redesign session management according to the selected Blazor hosting model.

Replace Web Forms-specific dependency injection configuration with ASP.NET Core dependency injection.

Review legacy request validation settings and implement appropriate input validation and output encoding in the modern application.

Migrate database configuration and evaluate Entity Framework Core compatibility.

Do not directly copy legacy `system.web` configuration into the modern application.

## 11. Other Legacy Technologies

The following technologies are not necessarily System.Web dependencies themselves, but they require consideration during modernization.

| Technology | Legacy Usage | Modernization Consideration |
|---|---|---|
| Entity Framework 6 | Database initialization and data access | Evaluate migration to EF Core |
| `ConfigurationManager.AppSettings` | Application configuration | Use ASP.NET Core configuration |
| Autofac | Service registration | Integrate with ASP.NET Core DI |
| log4net | Application logging | Evaluate ASP.NET Core logging integration |

### Unused System.Web Imports in Model Classes

#### Affected files:
- `Models/CatalogBrand.cs`
- `Models/CatalogType.cs`
- `Models/CatalogItemHiLoGenerator.cs`
- `Models/Infrastructure/PreconfiguredData.cs`

All four files import the `System.Web` namespace without using any `System.Web` APIs.

`CatalogBrand` and `CatalogType` define simple data models.

`CatalogItemHiLoGenerator` implements HiLo-based ID generation using Entity Framework 6 and a SQL Server sequence.

`PreconfiguredData` provides predefined catalog items, brands, and types for application initialization.

Migration approach: Remove the unused `using System.Web;` directives. These classes do not require Web Forms-specific functionality.

The HiLo generator's database dependencies should be reviewed separately during data access migration. The predefined catalog data can be reused in the modern application if needed.

## 12. Verification Status and Remaining Checks

The initial search identified 96 textual matches for `System.Web` across 29 files under the legacy directory.

These matches include active framework dependencies, project references, configuration entries, and unused namespace imports. Therefore, the match count does not represent 96 separate runtime dependencies.

The following areas have been reviewed:

- Web Forms pages, master pages, and user controls.
- Application startup, routing, session management, and resource bundling.
- File system and hosting dependencies.
- Mobile view switching and Friendly URLs.
- `Web.config`, `Web.Debug.config`, and `Web.Release.config`.
- `ApplicationInsights.config` and `eShopLegacyWebForms.csproj`.
- Model classes containing unused `System.Web` imports.
- Generated Web Forms designer files as a dependency category.

Additional verification may still be required for:

- Individual declarations in generated designer files that have not been inspected line by line.
- Indirect framework dependencies not captured by the textual `System.Web` search.
- Compatibility of legacy third-party packages with the target ASP.NET Core and Blazor versions.

Unused imports, configuration references, and active runtime dependencies should remain clearly distinguished.

## 13. Conclusion

The legacy eShop application relies on System.Web for page rendering, server controls, routing, session management, request processing, resource bundling, and application path resolution.

Migrating to Blazor requires replacing these framework-specific mechanisms with supported ASP.NET Core and Blazor features.

This inventory provides an initial migration reference and identifies areas requiring further verification.