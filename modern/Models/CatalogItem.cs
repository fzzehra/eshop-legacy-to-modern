using System.ComponentModel.DataAnnotations;

namespace EShop.Modern.Models;

public class CatalogItem
{
    public const string DefaultPictureName = "dummy.png";

    public int Id { get; set; }

    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [PriceRange(ErrorMessage = "The Price must be a positive number with maximum two decimals between 0 and 1 million.")]
    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = DefaultPictureName;

    public int CatalogTypeId { get; set; }

    public CatalogType? CatalogType { get; set; }

    public int CatalogBrandId { get; set; }

    public CatalogBrand? CatalogBrand { get; set; }

    [Range(0, 10_000_000, ErrorMessage = "The field Stock must be between 0 and 10 million.")]
    public int AvailableStock { get; set; }

    [Range(0, 10_000_000, ErrorMessage = "The field Restock must be between 0 and 10 million.")]
    public int RestockThreshold { get; set; }

    [Range(0, 10_000_000, ErrorMessage = "The field Max stock must be between 0 and 10 million.")]
    public int MaxStockThreshold { get; set; }

    public bool OnReorder { get; set; }
}
