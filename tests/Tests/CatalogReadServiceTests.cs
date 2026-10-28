using EShop.Modern.Data;
using EShop.Modern.Models;
using EShop.Modern.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class CatalogReadServiceTests
{
    private static CatalogDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new CatalogDbContext(options);

        var brand = new CatalogBrand { Id = 1, Brand = ".NET" };
        var type = new CatalogType { Id = 1, Type = "Mug" };

        db.CatalogBrands.Add(brand);
        db.CatalogTypes.Add(type);
        db.CatalogItems.AddRange(
            new CatalogItem
            {
                Id = 1,
                Name = ".NET Bot Black Sweatshirt",
                Price = 19.5m,
                PictureFileName = "1.png",
                CatalogBrandId = brand.Id,
                CatalogTypeId = type.Id,
                AvailableStock = 100
            },
            new CatalogItem
            {
                Id = 2,
                Name = ".NET Black & White Mug",
                Price = 8.5m,
                PictureFileName = "2.png",
                CatalogBrandId = brand.Id,
                CatalogTypeId = type.Id,
                AvailableStock = 89
            });
        db.SaveChanges();

        return db;
    }

    [Fact]
    public async Task GetCatalogItemsAsync_ReturnsAllItemsWithBrandAndType()
    {
        using var db = CreateContext();
        var service = new CatalogReadService(db);

        var items = await service.GetCatalogItemsAsync();

        Assert.Equal(2, items.Count);
        Assert.All(items, item =>
        {
            Assert.NotNull(item.CatalogBrand);
            Assert.NotNull(item.CatalogType);
        });
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_ReturnsMatchingItem()
    {
        using var db = CreateContext();
        var service = new CatalogReadService(db);

        var item = await service.GetCatalogItemByIdAsync(2);

        Assert.NotNull(item);
        Assert.Equal(".NET Black & White Mug", item!.Name);
        Assert.Equal(".NET", item.CatalogBrand?.Brand);
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_ReturnsNullForUnknownId()
    {
        using var db = CreateContext();
        var service = new CatalogReadService(db);

        var item = await service.GetCatalogItemByIdAsync(999);

        Assert.Null(item);
    }

    [Fact]
    public async Task GetCatalogItemsPaginatedAsync_ReturnsRequestedPage()
    {
        using var db = CreateContext();
        var service = new CatalogReadService(db);

        var page = await service.GetCatalogItemsPaginatedAsync(pageSize: 1, pageIndex: 1);

        Assert.Equal(1, page.ActualPage);
        Assert.Equal(2, page.TotalItems);
        Assert.Equal(2, page.TotalPages);
        Assert.Equal(2, Assert.Single(page.Data).Id);
    }

    [Fact]
    public async Task GetCatalogItemsPaginatedAsync_TreatsNegativeIndexAsFirstPage()
    {
        using var db = CreateContext();
        var service = new CatalogReadService(db);

        var page = await service.GetCatalogItemsPaginatedAsync(pageSize: 10, pageIndex: -1);

        Assert.Equal(0, page.ActualPage);
        Assert.Equal(2, page.Data.Count);
    }
}
