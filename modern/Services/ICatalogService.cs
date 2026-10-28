using EShop.Modern.Models;

namespace EShop.Modern.Services;

public interface ICatalogService
{
    Task<IReadOnlyList<CatalogBrand>> GetCatalogBrandsAsync();

    Task<IReadOnlyList<CatalogType>> GetCatalogTypesAsync();

    Task CreateCatalogItemAsync(CatalogItem catalogItem);

    Task UpdateCatalogItemAsync(CatalogItem catalogItem);

    Task RemoveCatalogItemAsync(CatalogItem catalogItem);
}
