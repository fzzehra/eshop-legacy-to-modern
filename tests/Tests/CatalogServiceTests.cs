
using EShop.Modern.Data;
using EShop.Modern.Models;
using EShop.Modern.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class CatalogServiceTests
{
    private static CatalogDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CatalogDbContext(options);
    }

    private static CatalogItem CreateItem(int id, string name)
    {
        return new CatalogItem
        {
            Id = id,
            Name = name,
            Price = 10m,
            PictureFileName = "test.png",
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            AvailableStock = 10
        };
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_ReturnsBrandsOrderedById()
    {
        using var db = CreateContext();

        db.CatalogBrands.AddRange(
            new CatalogBrand { Id = 2, Brand = "Brand B" },
            new CatalogBrand { Id = 1, Brand = "Brand A" }
        );
        await db.SaveChangesAsync();

        var service = new CatalogService(db);
        var brands = await service.GetCatalogBrandsAsync();

        Assert.Equal(new[] { 1, 2 }, brands.Select(b => b.Id));
    }

    [Fact]
    public async Task GetCatalogTypesAsync_ReturnsTypesOrderedById()
    {
        using var db = CreateContext();

        db.CatalogTypes.AddRange(
            new CatalogType { Id = 2, Type = "Type B" },
            new CatalogType { Id = 1, Type = "Type A" }
        );
        await db.SaveChangesAsync();

        var service = new CatalogService(db);
        var types = await service.GetCatalogTypesAsync();

        Assert.Equal(new[] { 1, 2 }, types.Select(t => t.Id));
    }

    [Fact]
    public async Task CreateCatalogItemAsync_AssignsFirstIdWhenDatabaseIsEmpty()
    {
        using var db = CreateContext();
        var service = new CatalogService(db);
        var item = CreateItem(0, "First Product");

        await service.CreateCatalogItemAsync(item);

        Assert.Equal(1, item.Id);
        Assert.Equal("First Product",
            (await db.CatalogItems.SingleAsync()).Name);
    }

    [Fact]
    public async Task CreateCatalogItemAsync_AssignsNextAvailableId()
    {
        using var db = CreateContext();

        db.CatalogItems.Add(CreateItem(5, "Existing Product"));
        await db.SaveChangesAsync();

        var service = new CatalogService(db);
        var item = CreateItem(0, "New Product");

        await service.CreateCatalogItemAsync(item);

        Assert.Equal(6, item.Id);
        Assert.Equal(2, await db.CatalogItems.CountAsync());
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_UpdatesExistingProduct()
    {
        using var db = CreateContext();

        db.CatalogItems.Add(CreateItem(1, "Original Product"));
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        var service = new CatalogService(db);
        var updated = CreateItem(1, "Updated Product");

        await service.UpdateCatalogItemAsync(updated);

        db.ChangeTracker.Clear();

        var saved = await db.CatalogItems.SingleAsync();

        Assert.Equal("Updated Product", saved.Name);
    }

    [Fact]
    public async Task RemoveCatalogItemAsync_RemovesExistingProduct()
    {
        using var db = CreateContext();

        var item = CreateItem(1, "Product to Delete");
        db.CatalogItems.Add(item);
        await db.SaveChangesAsync();

        var service = new CatalogService(db);

        await service.RemoveCatalogItemAsync(item);

        Assert.Empty(await db.CatalogItems.ToListAsync());
    }
}
