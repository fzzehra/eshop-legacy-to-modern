# E2E Regression Testleri

Playwright (TypeScript) ile yazılmış tarayıcı testleri. Aynı testler hem legacy hem yeni uygulamada çalışır.

## Kurulum (bir kere)

```powershell
cd tests/e2e
npm install
npx playwright install chromium
```

## Çalıştırma

**Legacy uygulamada:** Visual Studio'da legacy uygulama **Ctrl+F5 (Hata Ayıklama Olmadan Başlat)** ile çalışıyor olmalı. F5 ile başlatılırsa, uygulamayı çökerten testlerde Visual Studio uygulamayı durdurur ve diğer bütün testler zaman aşımına uğrar.

```powershell
npx playwright test
```

Varsayılan adres `http://localhost:50586`. Sende port farklıysa:

```powershell
$env:BASE_URL="http://localhost:PORT"; npx playwright test
```

**Yeni uygulamada:** Aynı komut, adres yeni uygulamanınki olacak şekilde.

```powershell
$env:BASE_URL="http://localhost:YENI_PORT"; $env:TARGET="modern"; npx playwright test
```

**Raporu görmek için:**

```powershell
npx playwright show-report
```

## Test verisi kuralları

- **Başlangıç verisi:** Uygulamanın ilk açılışta oluşturduğu ürünler (.NET Bot Black Hoodie vb.).
- Listeleme ve detay testleri sadece başlangıç ürünlerini okur.
- Başlangıç ürünleri hiçbir testte **değiştirilmez ve silinmez**.
- Ekleme, düzenleme ve silme testleri kendi ürününü oluşturur. Ürün adı `E2E-` ile başlar ve benzersiz olur (örn. `E2E-1730052000000`). Test sonunda bu ürün silinir.
- k6 yük testleri de aynı kurala uyar: başlangıç ürünlerini değiştirmez, kendi oluşturduğu ürünleri sonunda temizler.

## Seçici (selector) kuralı

- Web Forms'un ürettiği ID'ler (`ctl00_MainContent...` gibi) **kullanılmaz**, çünkü yeni uygulamada olmayacaklar.
- Görünen metin, buton/bağlantı adı ve form etiketleri kullanılır: `getByText`, `getByRole`, `getByLabel`.

## Bilinen legacy hataları

Legacy'deki bazı hatalar yeni sistemde düzeltilecek. Bu durumlar için yazılan testler **doğru davranışı** bekler ve `test.fail(isLegacy, ...)` ile işaretlenir:

- Legacy'de çalıştırıldığında başarısız olmaları beklenir; Playwright bunları başarılı sayar.
- Yeni sistemde (`TARGET=modern`) normal test gibi çalışır ve geçmeleri gerekir.

Yeni sistemde düzeltilecek davranışlar (ekip kararı):

| Durum | Legacy | Yeni sistem |
|---|---|---|
| Son sayfada "Showing 10 of 12" | Yanlış sayı | Gerçek sayı yazmalı ("Showing 2 of 12") |
| Negatif sayfa indeksi | Çöküyor | Çökmemeli (500 dönmemeli) |
| Olmayan veya geçersiz ürün id'si | Çöküyor | 404 dönmeli |
| Hata sayfalarında kaynak kodu | Görünüyor | Görünmemeli ("Server Error" sayfası olmamalı) |