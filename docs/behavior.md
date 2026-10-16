# Legacy Uygulama – Davranış Dokümanı: Edit ve Delete

Kayıt tarihi: 16.10.2026 · Kaydeden: Fatma Zehra Anbar
Ortam: `UseMockData=false` (LocalDB), Visual Studio 2022, IIS Express, Ctrl+F5 (hata ayıklayıcı olmadan)
Başlangıç verisi: bkz. [seed-data.md](seed-data.md)

---

## 1. Product Edit

### 1.1 Erişim
- URL: `/Catalog/Edit/{id}` (ör. `/Catalog/Edit/1`). id, route parametresi olarak alınır (query string değil).
- Liste sayfasındaki her satırda ve Details sayfasında **Edit** bağlantısı bulunur.

### 1.2 Ekran
- Solda ürün resmi (`/Pics/{PictureFileName}`), sağda form.
- Alanlar ürünün mevcut değerleriyle dolu gelir:

| Alan | Kontrol | Not |
|---|---|---|
| Name | metin kutusu | |
| Description | metin kutusu | |
| Brand | açılır liste | 5 markanın tamamı (kullanılmayanlar dahil), ürünün markası seçili |
| Type | açılır liste | 4 türün tamamı (kullanılmayanlar dahil), ürünün türü seçili |
| Price | metin kutusu | `$` işareti yok, 2 ondalık (ör. `19.50`) |
| Picture name | metin kutusu | **salt okunur** (gri) |
| Stock | metin kutusu | |
| Restock | metin kutusu | |
| Max stock | metin kutusu | |

- `OnReorder` alanı gösterilmez.
- Düğmeler: `[ CANCEL ]` (kırmızı), `[ SAVE ]` (yeşil).

### 1.3 Normal akış
- **SAVE** (geçerli verilerle) → değişiklik kaydedilir, **ana listeye (1. sayfa)** yönlendirilir. Başarı mesajı gösterilmez.
- **CANCEL** → kaydetmeden ana listeye (1. sayfa) döner. Kullanıcının geldiği sayfa hatırlanmaz.

### 1.4 Doğrulama kuralları
Hata mesajları ilgili alanın **hemen altında, kırmızı** gösterilir. Form aynı sayfada kalır, girilen geçersiz değer kutuda korunur.

| Alan | Girilen | Sonuç | Mesaj |
|---|---|---|---|
| Name | boş | ❌ | `The Name field is required.` |
| Name | 50 karakter | ✅ kaydedilir | — (listede tam olarak gösterilir) |
| Name | 51 karakter | 💥 **hata sayfası** | bkz. 1.5 |
| Description | boş | ✅ kaydedilir | — |
| Price | `-5` | ❌ | `The Price must be a positive number with maximum two decimals between 0 and 1 million.` |
| Price | `abc` | ❌ | aynı mesaj |
| Price | `1.999` | ❌ | aynı mesaj |
| Price | `1000001` | ❌ | aynı mesaj |
| Price | `0` | ✅ | `0.00` olarak kaydedilir |
| Price | `1000000` | ✅ | `1000000.00` olarak kaydedilir |
| Price | `12,50` | ✅ | `12.50` olarak kaydedilir (virgül ondalık ayırıcı kabul edilir) |
| Stock | `-1` | ❌ | `The field Stock must be between 0 and 10 million.` |
| Stock | `abc` | ❌ | aynı mesaj |
| Stock | `10000001` | ❌ | aynı mesaj |
| Restock | `-1` | ❌ | `The field Restock must be between 0 and 10 million.` |
| Max stock | `-1` | ❌ | `The field Max stock must be between 0 and 10 million.` |
| Brand / Type | `Azure` / `USB Memory Stick` | ✅ | kaydedilir, listede doğru gösterilir |

Notlar:
- Price kuralı özel bir mesaj kullanır; stok alanları standart `[Range]` mesaj kalıbını kullanır ("The field X must be between…").
- Mesajlarda alanın ekrandaki adı kullanılır ("Max stock"), kod adı (`MaxStockThreshold`) değil.
- Price ve stok alanlarında sınır değerler (0, 1.000.000 ve 10.000.000) **dahildir**.

### 1.5 Hata durumları (ele alınmamış istisnalar)
Aşağıdaki durumların hepsinde ASP.NET'in varsayılan **"Server Error in '/' Application"** sayfası gösterilir. Sayfa kaynak kodu, dosya yolları ve yığın izini içerir.

| Durum | İstisna | Kaynak |
|---|---|---|
| `/Catalog/Edit/999` (olmayan ürün) | `NullReferenceException` | `Edit.aspx.cs:28` – `product.CatalogBrandId.ToString()` |
| `/Catalog/Edit/abc` | `FormatException` | `Edit.aspx.cs:23` – `Convert.ToInt32(Page.RouteData.Values["id"])` |
| Name > 50 karakter → SAVE | `DbEntityValidationException` ("Validation failed for one or more entities…") | `CatalogService.cs:62` – `db.SaveChanges()` |

- 50 karakter sınırı formda kontrol edilmez; yalnızca EF6 kayıt sırasında kontrol eder ve uygulama çöker. Ürün kaydedilmez.
- Hata sayfasında **F5** formu yeniden gönderir (POST) ve aynı hata tekrar oluşur.

---

## 2. Product Delete

### 2.1 Erişim
- URL: `/Catalog/Delete/{id}` (ör. `/Catalog/Delete/12`).
- **Delete** bağlantısı yalnızca liste sayfasındaki satırlarda bulunur (Details sayfasında yok).

### 2.2 Onay ekranı
- Başlık: `Are you sure you want to delete this?`
- Ürün resmi ve tüm alanlar salt okunur gösterilir: Name, Description, Brand, Type, Price (ör. `$12.00`), Picture name, Stock, Restock, Max stock.
- Düğmeler: `[ CANCEL ]` (kırmızı), `[ DELETE ]` (**yeşil**).

### 2.3 Normal akış
- **DELETE** → ürün veritabanından silinir, **ana listeye (1. sayfa)** yönlendirilir. Onay veya başarı mesajı gösterilmez.
- Toplam ürün sayısı güncellenir (12 ürün → "Showing 10 of 11 products").
- **CANCEL** → silmeden ana listeye (1. sayfa) döner. Kullanıcının geldiği sayfa hatırlanmaz.

### 2.4 Sayfalama üzerindeki etkisi
- Id 12 silindikten sonra 2. sayfada 1 ürün kalır (`Cup<T> Sheet`).
- 2. sayfanın URL'si `/Default/index/1/size/10` (sayfa indeksi 0'dan başlar).
- 2. sayfanın alt yazısı: `Showing 10 of 11 products - Page 2 - 2`. Sayfada 1 ürün olduğu hâlde "Showing 10" yazar (mevcut hata, bkz. 3).

### 2.5 Hata durumları

| Durum | Sonuç | Kaynak |
|---|---|---|
| `/Catalog/Delete/999` (olmayan ürün) | 💥 `NullReferenceException` | `Delete.aspx:10` – `productToDelete.PictureFileName` |
| `/Catalog/Delete/abc` | 💥 `FormatException` | `Delete.aspx.cs:20` – `Convert.ToInt32(...)` |
| Silinmiş ürünün onay sayfasında (tarayıcı geri tuşu ile önbellekten) DELETE | 💥 `NullReferenceException` | `Delete.aspx:10`. Postback'te `Page_Load` ürünü yeniden arar, bulamaz; silme koduna ulaşılmaz |
| Silinmiş ürünün URL'si yeniden açılır (`/Catalog/Delete/12`) | 💥 `NullReferenceException` | `Delete.aspx:10` |

- Tarayıcı geri tuşuyla dönüldüğünde, silinmiş ürünün onay sayfası **önbellekten** hâlâ gösterilir.

---

## 3. Ekranlar arası ortak gözlemler
- **id doğrulaması yok:** Details, Edit ve Delete ekranlarında id `Convert.ToInt32` ile alınır ve ürünün bulunup bulunmadığı kontrol edilmez. Sayı olmayan id → `FormatException`; olmayan ürün → `NullReferenceException`.
- id'siz URL (ör. `/Catalog/Details`) → IIS **404.0 Not Found** sayfası (rota eşleşmez).
- Hata sayfaları kaynak kodu ve fiziksel dosya yollarını gösterir (`customErrors` kapalı) → **güvenlik bulgusu**.
- Liste sayfasındaki "Showing 10 of N" yazısı, son sayfada daha az ürün olsa da her zaman 10 gösterir.
- Footer: liste ve Details sayfalarının ilk açılışında Windows kullanıcı adı ve saat görünür; Create, Delete ve işlem sonrası açılan liste sayfalarında yalnızca `.` görünür.
- Her sayfa yüklemesinde log4net ile log yazılır (ör. `Now loading... /Catalog/Edit.aspx?id=...`).

## 4. Ekip kararı gerektiren noktalar
Yeni sistemde aşağıdaki davranışlar **birebir korunacak mı**, yoksa **düzeltilecek mi**?
1. Olmayan ürün / geçersiz id → hata sayfası mı, 404 mü?
2. Name > 50 karakter → çökme mi, form doğrulama mesajı mı?
3. "Showing 10 of N" sayım hatası
4. Mock ve DB modları arasındaki fiyat biçimi farkı (testler DB moduna göre yazılmalı)