using EShop.Modern.Models;

namespace EShop.Modern.Services;

public interface ICatalogReadService
{
    Task<IReadOnlyList<CatalogItem>> GetCatalogItemsAsync();

    Task<CatalogItem?> GetCatalogItemByIdAsync(int id);
}
