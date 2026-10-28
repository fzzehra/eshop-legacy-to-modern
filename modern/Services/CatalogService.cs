using EShop.Modern.Data;
using EShop.Modern.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Modern.Services;

public class CatalogService : ICatalogService
{
    private readonly CatalogDbContext _db;

    public CatalogService(CatalogDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CatalogBrand>> GetCatalogBrandsAsync()
    {
        return await _db.CatalogBrands.OrderBy(b => b.Id).ToListAsync();
    }

    public async Task<IReadOnlyList<CatalogType>> GetCatalogTypesAsync()
    {
        return await _db.CatalogTypes.OrderBy(t => t.Id).ToListAsync();
    }

    public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
    {
        var maxId = await _db.CatalogItems.MaxAsync(i => (int?)i.Id) ?? 0;
        catalogItem.Id = maxId + 1;

        _db.CatalogItems.Add(catalogItem);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCatalogItemAsync(CatalogItem catalogItem)
    {
        _db.Entry(catalogItem).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task RemoveCatalogItemAsync(CatalogItem catalogItem)
    {
        _db.CatalogItems.Remove(catalogItem);
        await _db.SaveChangesAsync();
    }
}
