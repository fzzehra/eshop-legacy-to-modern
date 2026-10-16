# Legacy Uygulama – Başlangıç Verisi (Seed Data)

Kayıt tarihi: 16.10.2026 · Kaydeden: Fatma Zehra Anbar
Kaynak: `legacy/eShopLegacyWebFormsSolution` (UseMockData=false, ilk çalıştırma, değişiklik yapılmadan önce)

## Ortam
- Veritabanı sunucusu: `(localdb)\MSSQLLocalDB` (SQL Server 2019, 15.0.4382)
- Veritabanı adı: `Microsoft.eShopOnContainers.Services.CatalogDb`
- Bağlantı dizesi adı (Web.config): `CatalogDBContext`
- Veritabanı, uygulamanın ilk açılışında EF6 tarafından otomatik oluşturulur ve doldurulur.
- Sıfırlamak için: SQL Server Object Explorer'da veritabanını sil → uygulamayı tekrar çalıştır.

## Tablolar
`Catalog`, `CatalogBrand`, `CatalogType`, `__MigrationHistory` (EF6 iç tablosu)

## Özet
| | Sayı |
|---|---|
| Ürün | 12 |
| Marka | 5 (yalnızca 2'si kullanılıyor) |
| Tür | 4 (yalnızca 3'ü kullanılıyor) |
| Sayfa boyutu | 10 → 2 sayfa (10 + 2) |

## Markalar (CatalogBrand)
| Id | Brand | Ürün sayısı |
|---|---|---|
| 1 | Azure | 0 |
| 2 | .NET | 6 |
| 3 | Visual Studio | 0 |
| 4 | SQL Server | 0 |
| 5 | Other | 6 |

## Türler (CatalogType)
| Id | Type | Ürün sayısı |
|---|---|---|
| 1 | Mug | 2 |
| 2 | T-Shirt | 7 |
| 3 | Sheet | 3 |
| 4 | USB Memory Stick | 0 |

## Ürünler (Catalog)
Tüm ürünlerde: AvailableStock = 100, RestockThreshold = 0, MaxStockThreshold = 0, OnReorder = 0. Description = Name.

| Id | Name | Price | PictureFileName | Type | Brand |
|---|---|---|---|---|---|
| 1 | .NET Bot Black Hoodie | 19.50 | 1.png | T-Shirt | .NET |
| 2 | .NET Black & White Mug | 8.50 | 2.png | Mug | .NET |
| 3 | Prism White T-Shirt | 12.00 | 3.png | T-Shirt | Other |
| 4 | .NET Foundation T-shirt | 12.00 | 4.png | T-Shirt | .NET |
| 5 | Roslyn Red Sheet | 8.50 | 5.png | Sheet | Other |
| 6 | .NET Blue Hoodie | 12.00 | 6.png | T-Shirt | .NET |
| 7 | Roslyn Red T-Shirt | 12.00 | 7.png | T-Shirt | Other |
| 8 | Kudu Purple Hoodie | 8.50 | 8.png | T-Shirt | Other |
| 9 | Cup<T> White Mug | 12.00 | 9.png | Mug | Other |
| 10 | .NET Foundation Sheet | 12.00 | 10.png | Sheet | .NET |
| 11 | Cup<T> Sheet | 8.50 | 11.png | Sheet | .NET |
| 12 | Prism White TShirt | 12.00 | 12.png | T-Shirt | Other |

## Şema (Catalog)
| Kolon | Tip | Null olabilir mi |
|---|---|---|
| Id | int | Hayır |
| Name | nvarchar(50) | Hayır |
| Description | nvarchar(max) | **Evet** |
| Price | decimal(18,2) | Hayır |
| PictureFileName | nvarchar(max) | Hayır |
| CatalogTypeId | int (FK) | Hayır |
| CatalogBrandId | int (FK) | Hayır |
| AvailableStock | int | Hayır |
| RestockThreshold | int | Hayır |
| MaxStockThreshold | int | Hayır |
| OnReorder | bit | Hayır |

`CatalogBrand.Brand` ve `CatalogType.Type`: nvarchar(100), null olamaz.

## Mock veri ile farklar (UseMockData=true)
- Ürünler, sıralama ve sayfalama aynı (12 ürün, 10/2).
- **Fiyat gösterimi farklı:** mock modda `$19.5`, `$12`, `$8.5`; veritabanı modunda `$19.50`, `$12.00`, `$8.50`.
- Testler veritabanı moduna göre yazılmalıdır.

## Test açısından dikkat çeken noktalar
- Kullanılmayan marka (Azure, Visual Studio, SQL Server) ve tür (USB Memory Stick) var; Create ve Edit açılır listelerinde **görünür**