using EShop.Modern.Data;
using EShop.Modern.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Modern.Services;

public class CatalogReadService : ICatalogReadService
{
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

    public async Task<CatalogItem?> GetCatalogItemByIdAsync(int id)
    {
        return await _db.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}
