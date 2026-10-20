# Legacy Uygulama – Davranış Dokümanı
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
| `/Catalog/Delete/999` (olmayan ürün) |  `NullReferenceException` | `Delete.aspx:10` – `productToDelete.PictureFileName` |
| `/Catalog/Delete/abc` |  `FormatException` | `Delete.aspx.cs:20` – `Convert.ToInt32(...)` |
| Silinmiş ürünün onay sayfasında (tarayıcı geri tuşu ile önbellekten) DELETE |  `NullReferenceException` | `Delete.aspx:10`. Postback'te `Page_Load` ürünü yeniden arar, bulamaz; silme koduna ulaşılmaz |
| Silinmiş ürünün URL'si yeniden açılır (`/Catalog/Delete/12`) |  `NullReferenceException` | `Delete.aspx:10` |

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

---

## 5. Product List

Kayıt tarihi: 17.10.2026 · Kaydeden: Sinem  
Ortam: `UseMockData=false` (LocalDB), Visual Studio 2022, IIS Express  
Başlangıç verisi: bkz. [seed-data.md](seed-data.md)

### 5.1 Erişim
---
- Ana adres: `/`
- Birinci sayfa: `/Default/index/0/size/10`
- İkinci sayfa: `/Default/index/1/size/10`
- Sayfalama URL'sinde sayfa indeksi 0'dan başlar.

### 5.2 Ekran

Ürünler tablo biçiminde listelenir. Her ürün için aşağıdaki bilgiler gösterilir:

| Alan | Açıklama |
|---|---|
| Picture | Ürün görseli |
| Name | Ürün adı |
| Description | Ürün açıklaması |
| Brand | Marka |
| Type | Ürün türü |
| Price | `$` işareti ve iki ondalık basamak |
| Picture name | Görsel dosya adı |
| Stock | Stok miktarı |
| Restock | Yeniden stoklama değeri |
| Max stock | Maksimum stok değeri |

Her satırda `Edit`, `Details` ve `Delete` bağlantıları bulunur. Listenin üstünde `CREATE NEW` düğmesi vardır.

### 5.3 Normal akış ve sayfalama

- Başlangıçta veritabanında 12 ürün bulunurken ilk sayfada 10 ürün gösterildi.
- İkinci sayfada kalan 2 ürün gösterildi.
- `Next` bağlantısı ikinci sayfaya geçiş yapar.
- `Previous` bağlantısı birinci sayfaya döner.
- Birinci sayfanın alt yazısı: `Showing 10 of 12 products - Page 1 - 2`
- İkinci sayfanın alt yazısı: `Showing 10 of 12 products - Page 2 - 2`

İkinci sayfada yalnızca 2 ürün bulunmasına rağmen `Showing 10` yazısı değişmez. Bu durum mevcut uygulamanın sayfalama gösterim hatasıdır.

### 5.4 Sınır durumları

| Durum | Gözlemlenen sonuç |
|---|---|
| Sayfa indeksi `0` | İlk 10 ürün gösterilir |
| Sayfa indeksi `1` | Sonraki 2 ürün gösterilir |
| Sayfa indeksi `99` | `No data was returned.` mesajı gösterilir |
| Sayfa indeksi `99` | Alt yazı `Showing 10 of 12 products - Page 100 - 2` olur |
| Sayfa indeksi `-1` | `System.ArgumentException`: `Count must have a non-negative value. Parameter name: count`. `CatalogService.cs:26` satırında hata oluşur. Ctrl+F5 ile doğrulanmıştır. |
---

## 6. Product Details

### 6.1 Erişim

- URL: `/Catalog/Details/{id}`
- Örnek: `/Catalog/Details/1`
- Liste sayfasındaki `Details` bağlantısıyla açılır.

### 6.2 Ekran

Ürün detay sayfasında solda ürün görseli, sağda ürün bilgileri bulunur.

Gösterilen alanlar:

- Name
- Description
- Brand
- Type
- Price
- Picture name
- Stock
- Restock
- Max stock

Alanlar salt okunurdur. Sayfanın altında `Edit` ve `Back to list` bağlantıları bulunur.

### 6.3 Normal akış

`/Catalog/Details/1` adresi açıldığında `.NET Bot Black Hoodie` ürününün bilgileri görüntülendi.

Örnek değerler:

| Alan | Değer |
|---|---|
| Name | .NET Bot Black Hoodie |
| Brand | .NET |
| Type | T-Shirt |
| Price | $19.50 |
| Picture name | 1.png |
| Stock | 100 |
| Restock | 0 |
| Max stock | 0 |

`Edit` bağlantısı düzenleme ekranına, `Back to list` bağlantısı ürün listesine geçiş sağlar.

### 6.4 Hata durumları

| Durum | Sonuç |
|---|---|
| `/Catalog/Details/999` | `NullReferenceException` |
| `/Catalog/Details/abc` | `FormatException` |

**Olmayan ürün:** `product` nesnesi `null` olduğundan `Details.aspx` dosyasındaki ürün görseli oluşturulurken hata meydana gelir.

**Geçersiz ID:** `Details.aspx.cs` dosyasında `Convert.ToInt32(Page.RouteData.Values["id"])` işlemi `FormatException` üretir.

Bu hatalar kullanıcı dostu bir mesaj yerine uygulamanın hata ayıklama/hata ekranına yol açmaktadır.

---

## 7. Product Create

### 7.1 Erişim

- URL: `/Catalog/Create`
- Liste sayfasındaki `CREATE NEW` düğmesiyle açılır.

### 7.2 Ekran

| Alan | Kontrol | Başlangıç durumu |
|---|---|---|
| Name | Metin kutusu | Boş |
| Description | Metin kutusu | Boş |
| Brand | Açılır liste | Azure |
| Type | Açılır liste | Mug |
| Price | Metin kutusu | `0.00` |
| Picture name | Bilgilendirme metni | Görsel yükleme desteklenmiyor |
| Stock | Metin kutusu | `0` |
| Restock | Metin kutusu | `0` |
| Max stock | Metin kutusu | `0` |

Görsel alanında `Uploading images not allowed for this version.` mesajı gösterilir.

Formun altında `[ CANCEL ]` (kırmızı) ve `[ CREATE ]` (yeşil) düğmeleri bulunur.

### 7.3 Normal akış

Geçerli verilerle `CREATE` düğmesine basıldığında:

- Ürün veritabanına kaydedilir.
- Kullanıcı doğrudan ürün listesinin birinci sayfasına yönlendirilir.
- Başarı mesajı gösterilmez.
- Yeni ürün, kayıt sayısına bağlı olarak ikinci sayfada görüntülenebilir.

Başarılı test kaydı:

| Alan | Değer |
|---|---|
| Name | Sinem Create Test |
| Description | Create successful test |
| Brand | Azure |
| Type | Mug |
| Price | 10.50 |
| Picture name | dummy.png |
| Stock | 5 |
| Restock | 2 |
| Max stock | 10 |

`CANCEL` düğmesi kullanıldığında ürün oluşturulmaz ve kullanıcı birinci liste sayfasına döner.

### 7.4 Doğrulama kuralları

Hata mesajları ilgili alanın altında kırmızı gösterilir. Geçersiz değerler düzeltilene kadar kayıt gerçekleşmez.

| Alan | Girilen değer | Sonuç |
|---|---|---|
| Name | Boş |  `The Name field is required.` |
| Name | 50 karakter |  Ürün oluşturuldu |
| Name | 51 karakter |  EF6 kayıt hatası |
| Price | `-5` |  Reddedildi |
| Price | `abc` |  Reddedildi |
| Price | `1.999` |  Reddedildi |
| Price | `1000001` |  Reddedildi |
| Price | `0` |  Ürün oluşturuldu |
| Price | `1000000` |  Ürün oluşturuldu |
| Price | `12,50` |  Reddedildi |
| Stock | `-1` |  Reddedildi |
| Stock | `0` |  Kabul edildi |
| Stock | `10000001` |  Reddedildi |
| Stock | `10000000` |  Ürün oluşturuldu |
| Restock | `-1` |  Reddedildi |
| Max stock | `-1` |  Reddedildi |

**Price hata mesajı:**

`The Price must be a positive number with maximum two decimals between 0 and 1 million.`

**Stock hata mesajı:**

`The field Stock must be between 0 and 10 million.`

**Restock hata mesajı:**

`The field Restock must be between 0 and 10 million.`

**Max stock hata mesajı:**

`The field Max stock must be between 0 and 10 million.`

### 7.5 Hata durumları ve önemli bulgular

**Name uzunluk sınırı:** 51 karakterlik bir ürün adı girildiğinde istemci tarafında doğrulama mesajı gösterilmez. `CatalogService.cs` dosyasındaki `db.SaveChanges()` işleminde `DbEntityValidationException` meydana gelir.

**Fiyat biçimi:** Create ekranında virgülle girilen `12,50` ve `12,5` değerleri reddedilmiştir. Tekrar yapılan testte `12,5` için `The Price must be a positive number with maximum two decimals between 0 and 1 million.` doğrulama mesajı gösterilmiştir. Edit ekranında ise `12,50` değerinin kabul edildiği raporlanmıştır. Bu farklılığın nedeni henüz belirlenmemiştir.

**Fiyat sınırları:** `0` ve `1000000` değerleri kabul edilmiştir; `1000001` reddedilmiştir.

**Stok sınırları:** `0` ve `10000000` kabul edilmiştir; `-1` ve `10000001` reddedilmiştir.

**Kayıt sonrası yönlendirme:** Başarılı kayıt sonrasında kullanıcı, ürünün bulunduğu sayfaya değil, birinci liste sayfasına yönlendirilir.

---

## 8. Modernizasyon sırasında değerlendirilmesi gereken noktalar
1. Ürün listeleme ve sayfalama davranışı regresyon testleriyle korunmalı; hatalı sayfa sayacı ayrıca ele alınmalıdır.
2. Geçersiz veya bulunamayan ürün ID'leri kullanıcı dostu biçimde yönetilmelidir.
3. Name uzunluk sınırı veritabanına kayıt yapılmadan önce doğrulanmalıdır.
4. Create ve Edit ekranlarındaki fiyat biçimi farklılığı için ortak bir kural belirlenmelidir.
5. Başarılı Create işlemi, CANCEL davranışı ve kayıt sonrası yönlendirme yeni Blazor uygulamasında test edilmelidir.

Bu maddeler mevcut sistemin gözlemlerinden türetilen önerilerdir; yeni uygulamanın kesin davranışları ekip kararıyla belirlenecektir.