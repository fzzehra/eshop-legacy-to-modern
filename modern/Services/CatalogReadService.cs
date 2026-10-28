using EShop.Modern.Data;
using EShop.Modern.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Modern.Services;

public class CatalogReadService : ICatalogReadService
{
    public const int DefaultPageSize = 10;

    private readonly CatalogDbContext _db;

    public CatalogReadService(CatalogDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CatalogItem>> GetCatalogItemsAsync()
    {
        return await _db.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<PaginatedItems<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex)
    {
        // Legacy crashed on a negative index or zero size; fall back to safe values instead.
        if (pageSize <= 0) pageSize = DefaultPageSize;
        if (pageIndex < 0) pageIndex = 0;

        var totalItems = await _db.CatalogItems.CountAsync();
        var items = await _db.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(i => i.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, items);
    }

    public async Task<CatalogItem?> GetCatalogItemByIdAsync(int id)
    {
        return await _db.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}
