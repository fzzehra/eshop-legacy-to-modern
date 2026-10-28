using EShop.Modern.Models;

namespace EShop.Modern.Services;

public interface ICatalogReadService
{
    Task<IReadOnlyList<CatalogItem>> GetCatalogItemsAsync();

    Task<PaginatedItems<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex);

    Task<CatalogItem?> GetCatalogItemByIdAsync(int id);
}
