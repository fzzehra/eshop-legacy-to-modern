# Legacy Uygulama – URL / Route Dokümanı

Kayıt tarihi: 17.10.2026 · Kaydeden: Sinem  
Ortam: `UseMockData=false` (LocalDB), IIS Express  
Yerel adres: `http://localhost:50586`

## 1. Ekran Adresleri

| Ekran | URL | Açıklama |
|---|---|---|
| Product List | `/` | Ürün listeleme ana sayfası |
| Product List – Page 1 | `/Default/index/0/size/10` | İlk 10 ürün |
| Product List – Page 2 | `/Default/index/1/size/10` | Sonraki ürünler |
| Product Details | `/Catalog/Details/{id}` | Ürün detaylarını görüntüleme |
| Product Create | `/Catalog/Create` | Yeni ürün oluşturma |
| Product Edit | `/Catalog/Edit/{id}` | Ürün düzenleme |
| Product Delete | `/Catalog/Delete/{id}` | Ürün silme onay ekranı |

## 2. Route Parametreleri

- `{id}`: İşlem yapılacak ürünün sayısal kimliği. Örnek: `/Catalog/Details/1`
- `index`: Sıfırdan başlayan sayfa indeksi.
- `size`: Sayfa başına gösterilecek ürün sayısı. Testlerde `10` kullanılmıştır.

## 3. Hatalı URL Davranışları

| URL | Gözlemlenen davranış |
|---|---|
| `/Default/index/99/size/10` | Boş liste ve `No data was returned.` mesajı |
| `/Default/index/-1/size/10` | `System.ArgumentException`: `Count must have a non-negative value. Parameter name: count`. `CatalogService.cs:26` (Ctrl+F5 ile doğrulandı). |
| `/Catalog/Details/999` | `NullReferenceException` |
| `/Catalog/Details/abc` | `FormatException` |
| `/Catalog/Edit/999` | `NullReferenceException` |
| `/Catalog/Edit/abc` | `FormatException` |
| `/Catalog/Delete/999` | `NullReferenceException` |
| `/Catalog/Delete/abc` | `FormatException` |

## 4. Modernizasyon Notu

Yeni ASP.NET Core + Blazor uygulamasında mevcut URL yapısı incelenerek eski ve yeni sayfalar arasındaki yönlendirmeler Strangler Fig yaklaşımı kapsamında değerlendirilecektir.