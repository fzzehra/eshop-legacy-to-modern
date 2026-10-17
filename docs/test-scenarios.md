# Test Senaryoları – Legacy Davranışının Korunması

Kaydeden: Seray · Kaynaklar: `legacy/` kodu, [behavior.md](behavior.md), [seed-data.md](seed-data.md), [legacy-screens/](legacy-screens/)

Bu senaryolar eski uygulamanın davranışını tanımlar. Yeni Blazor uygulaması aynı sonuçları vermelidir. Playwright testleri bu listeye göre yazılacaktır.

## Ön koşul (tüm senaryolar)
- Veritabanı başlangıç verisiyle sıfırlanmış: 12 ürün, 5 marka, 4 tür (bkz. [seed-data.md](seed-data.md)).
- `UseMockData=false` (LocalDB).
- Sayfa boyutu: 10.

**Tür:** ✅ başarılı akış · ❌ geçersiz girdi · ⚠️ sınır durumu

---

## 1. List (ürün listesi)

| ID | Tür | Adımlar | Beklenen sonuç |
|---|---|---|---|
| TS-01 | ✅ | `/` adresini aç | Başlık "Catalog manager (Web Forms)", **CREATE NEW** düğmesi, 10 ürün satırı. İlk satır: `.NET Bot Black Hoodie`, `.NET`, `T-Shirt`, `$19.50`, `1.png`, `100`, `0`, `0`. Her satırda `Edit \| Details \| Delete` bağlantıları. Alt yazı: `Showing 10 of 12 products - Page 1 - 2` |
| TS-02 | ✅ | 1. sayfada **Next**'e tıkla | URL `/Default/index/1/size/10`. 2 ürün görünür (Id 11, 12). **Previous** görünür, **Next** gizli. Alt yazı `... Page 2 - 2` |
| TS-03 | ⚠️ | 1. sayfayı aç | **Previous** bağlantısı gizli (`esh-pager-item--hidden`) |
| TS-04 | ⚠️ | 2. sayfayı aç | Alt yazı `Showing 10 of 12 products` (sayfada 2 ürün olsa da "10" yazar; bilinen hata) |
| TS-05 | ❌ | `/Default/index/abc/size/10` adresini aç | 💥 Hata sayfası (`FormatException`, `Convert.ToInt32`) |

## 2. Details (ürün detayı)

| ID | Tür | Adımlar | Beklenen sonuç |
|---|---|---|---|
| TS-06 | ✅ | Listede 1. ürünün **Details** bağlantısına tıkla | URL `/Catalog/Details/1`. Resim `/Pics/1.png`. Alanlar salt okunur: Name `.NET Bot Black Hoodie`, Brand `.NET`, Type `T-Shirt`, Price `$19.50`, Stock `100`. **Edit** bağlantısı var, **Delete** yok |
| TS-07 | ❌ | `/Catalog/Details/999` adresini aç | 💥 Hata sayfası (`NullReferenceException`; ürün bulunamadığı kontrol edilmiyor) |
| TS-08 | ❌ | `/Catalog/Details/abc` adresini aç | 💥 Hata sayfası (`FormatException`) |
| TS-09 | ⚠️ | `/Catalog/Details` adresini (id olmadan) aç | 404 Not Found |

## 3. Create (yeni ürün)

| ID | Tür | Adımlar | Beklenen sonuç |
|---|---|---|---|
| TS-10 | ✅ | **CREATE NEW** → Name `Test Mug`, Brand `Azure`, Type `Mug`, Price `9.99`, Stock `5` → **CREATE** | Ana listeye yönlendirilir. Alt yazı `of 13 products`. Yeni ürün 2. sayfada, Picture name `dummy.png` |
| TS-11 | ⚠️ | Create formunu aç | Varsayılan değerler: Price `0.00`, Stock/Restock/Max stock `0` |
| TS-12 | ❌ | Name boş bırak → **CREATE** | `The Name field is required.` mesajı alanın altında kırmızı gösterilir. Kayıt yapılmaz, ürün sayısı 12 kalır |
| TS-13 | ❌ | Price `-5` (ayrıca `abc`, `1000001`) → **CREATE** | `The Price must be a positive number with maximum two decimals between 0 and 1 million.` Kayıt yapılmaz |
| TS-14 | ❌ | Stock `-1` (ayrıca Restock `-1`, Max stock `10000001`) → **CREATE** | `The field Stock must be between 0 and 10 million.` (ilgili alan adıyla). Kayıt yapılmaz |
| TS-15 | ⚠️ | Price `0` ve `1000000`, Stock `10000000` → **CREATE** | Sınırlar dahil: ürün kaydedilir |
| TS-16 | ⚠️ | Name 50 karakter → **CREATE** · Name 51 karakter → **CREATE** | 50 karakter: kaydedilir. 51 karakter: 💥 Hata sayfası (`DbEntityValidationException`), kayıt yapılmaz |

## 4. Edit (ürün düzenleme)

| ID | Tür | Adımlar | Beklenen sonuç |
|---|---|---|---|
| TS-17 | ✅ | `/Catalog/Edit/1` → Price `25.00` → **SAVE** | Ana listeye (1. sayfa) yönlendirilir, 1. ürünün fiyatı `$25.00`. Başarı mesajı yok |
| TS-18 | ✅ | `/Catalog/Edit/1` → bir alanı değiştir → **CANCEL** | Listeye dönülür, değişiklik kaydedilmez |
| TS-19 | ⚠️ | `/Catalog/Edit/1` aç | Alanlar mevcut değerlerle dolu; Price `19.50` (`$` yok); Picture name salt okunur. Brand listesinde 5, Type listesinde 4 seçenek |
| TS-20 | ❌ | Name boş, Price `1.999` → **SAVE** | Alanların altında ilgili hata mesajları. Form sayfada kalır, girilen değerler korunur |
| TS-21 | ⚠️ | Price `12,50` → **SAVE** | Kabul edilir, `$12.50` olarak kaydedilir |
| TS-22 | ❌ | `/Catalog/Edit/999` ve `/Catalog/Edit/abc` adreslerini aç | 💥 Hata sayfası (`NullReferenceException` / `FormatException`) |

## 5. Delete (ürün silme)

| ID | Tür | Adımlar | Beklenen sonuç |
|---|---|---|---|
| TS-23 | ✅ | Listede 12. ürünün **Delete** bağlantısı → **DELETE** | Başlık `Are you sure you want to delete this?`. Silme sonrası listeye dönülür, alt yazı `of 11 products`; 2. sayfada yalnızca `Cup<T> Sheet` kalır |
| TS-24 | ✅ | `/Catalog/Delete/1` → **CANCEL** | Listeye dönülür, ürün silinmez, sayı 12 kalır |
| TS-25 | ❌ | `/Catalog/Delete/999` ve `/Catalog/Delete/abc` adreslerini aç | 💥 Hata sayfası (`NullReferenceException` / `FormatException`) |
| TS-26 | ⚠️ | Ürünü sil → tarayıcıda geri tuşu → tekrar **DELETE** | 💥 Hata sayfası (`NullReferenceException`; ürün artık yok) |

---

## Ekip kararı bekleyen senaryolar
Şu senaryolarda eski uygulama **hata sayfası** gösteriyor: TS-04, TS-05, TS-07, TS-08, TS-16 (51 karakter), TS-22, TS-25, TS-26. Yeni uygulamada bu davranış korunacak mı, yoksa 404 sayfası ya da form doğrulama mesajına mı çevrilecek? Bkz. [behavior.md §4](behavior.md). Karar verilince bu senaryoların "Beklenen sonuç" sütunu güncellenmelidir.
