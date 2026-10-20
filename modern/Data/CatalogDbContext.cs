using EShop.Modern.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Modern.Data;

// Maps to the existing legacy schema (Catalog, CatalogBrand, CatalogType tables).
// No schema changes: table names, key/required constraints, and max lengths mirror
// legacy/.../Models/CatalogDBContext.cs (EF6) exactly.
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();

    public DbSet<CatalogBrand> CatalogBrands => Set<CatalogBrand>();

    public DbSet<CatalogType> CatalogTypes => Set<CatalogType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogType>(builder =>
        {
            builder.ToTable("CatalogType");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogBrand>(builder =>
        {
            builder.ToTable("CatalogBrand");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Brand)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogItem>(builder =>
        {
            builder.ToTable("Catalog");
            builder.HasKey(i => i.Id);
            // Legacy assigns Id itself via CatalogItemHiLoGenerator, not a DB identity column.
            builder.Property(i => i.Id).ValueGeneratedNever();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.PictureFileName)
                .IsRequired();

            builder.HasOne(i => i.CatalogBrand)
                .WithMany()
                .HasForeignKey(i => i.CatalogBrandId)
                .IsRequired();

            builder.HasOne(i => i.CatalogType)
                .WithMany()
                .HasForeignKey(i => i.CatalogTypeId)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
