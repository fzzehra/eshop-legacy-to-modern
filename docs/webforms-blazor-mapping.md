# Web Forms to Blazor Mapping

## 1. Purpose

This document maps the main components, pages, controls, lifecycle methods, and navigation mechanisms of the legacy eShop Web Forms application to their proposed Blazor equivalents.

The goal is to support the modernization process while preserving the existing catalog management functionality.

## 2. Page Mapping

| Legacy Web Forms Page | Purpose | Proposed Blazor Equivalent |
|---|---|---|
| `Default.aspx` | Product listing and pagination | `Catalog.razor` |
| `Catalog/Create.aspx` | Create a new product | `Create.razor` |
| `Catalog/Details.aspx` | Display product details | `Details.razor` |
| `Catalog/Edit.aspx` | Edit an existing product | `Edit.razor` |
| `Catalog/Delete.aspx` | Confirm and delete a product | `Delete.razor` |
| `Site.Master` | Shared header, content area, and footer | `MainLayout.razor` |

The Blazor filenames and routes are proposed mappings and should be aligned with the structure of the modern application during implementation.

## 3. Server Control Mapping

| Web Forms Control | Usage in Legacy Application | Blazor Equivalent |
|---|---|---|
| `asp:ListView` | Display product rows | Razor `@foreach` |
| `asp:TextBox` | Edit product properties | `InputText`, `InputNumber` |
| `asp:DropDownList` | Select product brand and type | `InputSelect` |
| `asp:Label` | Display product information | Razor expressions |
| `asp:Image` | Display product pictures | HTML `img` |
| `asp:Button` | Create, save, and delete actions | HTML button or Blazor form submission |
| `asp:HyperLink` | Product navigation and pagination | HTML anchor or `NavLink` |
| `asp:RequiredFieldValidator` | Require product name | Data annotations with Blazor form validation |
| `asp:RangeValidator` | Validate price and stock ranges | Data annotations with Blazor form validation |
| `asp:ContentPlaceHolder` | Define shared content region | `@Body` in a layout |
| `asp:Content` | Supply page-specific content | Razor component content |
| `asp:ScriptManager` | Load Web Forms and JavaScript resources | Modern JavaScript asset loading and interop where needed |

## 4. Lifecycle and Data Binding Mapping

| Web Forms Mechanism | Legacy Usage | Blazor Approach |
|---|---|---|
| `Page_Load` | Load catalog data and initialize pages | `OnInitializedAsync` or `OnParametersSetAsync` |
| `Page.IsPostBack` | Avoid resetting form fields on postback | Component state and lifecycle management |
| `DataSource` | Assign data to server controls | Bind collections to Razor rendering |
| `DataBind()` | Populate controls with model values | Declarative Razor rendering and bindings |
| `<%# ... %>` | Bind product properties to controls | `@product.Property` and `@bind-Value` |
| `ModelState.IsValid` | Check submitted form validity | `EditForm`, `DataAnnotationsValidator`, and `OnValidSubmit` |
| `OnClick` server events | Handle create, save, and delete operations | Blazor event handlers and form callbacks |

## 5. Routing and Navigation

The legacy application uses `Page.RouteData.Values` to retrieve route parameters, `GetRouteUrl()` to generate links, and `Response.Redirect()` for navigation.

In Blazor, page routes can be declared with `@page`, route values can be received through `[Parameter]`, and navigation can be performed using `NavigationManager`.

For example, a product details page could use:

`@page "/catalog/details/{Id:int}"`

The existing product ID would then be received as a component parameter.

## 6. Catalog Operations

### 6.1 Product Listing

**Legacy files:** `Default.aspx`, `Default.aspx.cs`

The page retrieves paginated catalog items using `CatalogService.GetCatalogItemsPaginated()`. Results are displayed through `asp:ListView`, with links to the create, details, edit, and delete pages.

Pagination uses route values for page index and page size. The default page index is `0`, and the default page size is `10`.

**Blazor approach:** Load a paginated collection through a service and render product rows with Razor. Use route or query parameters to manage pagination.

### 6.2 Product Creation

**Legacy files:** `Catalog/Create.aspx`, `Catalog/Create.aspx.cs`

The page provides inputs for product name, description, brand, type, price, and stock properties. Product creation is handled by `Create_Click()`, which calls `CatalogService.CreateCatalogItem()`.

**Blazor approach:** Use an `EditForm` with a validated input model. Load brand and type options from a service and submit the new product through a create operation.

### 6.3 Product Details

**Legacy files:** `Catalog/Details.aspx`, `Catalog/Details.aspx.cs`

The product ID is retrieved from route data. `CatalogService.FindCatalogItem()` loads the selected product, and `DataBind()` displays its properties.

**Blazor approach:** Receive the product ID as a route parameter, retrieve the product through a service, and display its properties with Razor expressions.

### 6.4 Product Editing

**Legacy files:** `Catalog/Edit.aspx`, `Catalog/Edit.aspx.cs`

The page loads an existing product and its brand and type selections. `Page.IsPostBack` prevents initial data binding from overwriting submitted form values. The `Save_Click()` handler validates the form and calls `CatalogService.UpdateCatalogItem()`.

The picture filename is displayed as read-only.

**Blazor approach:** Load the product into an editable model, use two-way binding and form validation, preserve the read-only picture filename behavior, and call an update service when the form is valid.

### 6.5 Product Deletion

**Legacy files:** `Catalog/Delete.aspx`, `Catalog/Delete.aspx.cs`

The page displays the selected product and requests confirmation before deletion. The `Delete_Click()` handler calls `CatalogService.RemoveCatalogItem()` and redirects to the catalog list.

**Blazor approach:** Display the selected product in a confirmation page, invoke a delete operation after user confirmation, and navigate back to the catalog list.

## 7. Shared Layout Mapping

**Legacy files:** `Site.Master`, `Site.Master.cs`

The legacy master page defines the shared application layout:

- Header with brand logo
- Catalog manager title section
- `MainContent` placeholder
- Footer with branding and session information

The master page also uses `asp:ScriptManager` to load JavaScript resources.

In `Site.Master.cs`, `HttpContext.Current.Session` is used to read `MachineName` and `SessionStartTime`, which are displayed in the footer.

**Blazor approach:** Create a shared `MainLayout.razor` component with a header, navigation area, `@Body`, and footer. Handle session-related information through an appropriate ASP.NET Core or application state service instead of relying on `System.Web.HttpContext.Current`.

## 8. Modernization Considerations

The migration should address the following differences:

- Web Forms server controls must be replaced with Razor markup and Blazor components.
- Page postbacks must be replaced with Blazor event handling and component state management.
- Form validation should be implemented using a suitable Blazor validation model.
- Routing and navigation must use ASP.NET Core and Blazor mechanisms.
- `System.Web` dependencies must be removed or replaced with supported ASP.NET Core approaches.
- Legacy session handling must be redesigned for the selected Blazor hosting model.
- Existing catalog functionality, validation rules, and user-facing workflows should be preserved.
- Missing-product scenarios, invalid route parameters, and service errors should be handled explicitly.
- Existing CSS classes and image assets may be reused where compatible to preserve the application's appearance.

## 9. Conclusion

The legacy eShop application uses Web Forms pages, server controls, code-behind event handlers, master pages, and session-based functionality to implement catalog management.

Blazor provides corresponding functionality through Razor components, shared layouts, route parameters, component lifecycle methods, form validation, and event-driven interaction.

This mapping serves as a reference for implementing and verifying the modern catalog application.