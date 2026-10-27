# E2E Regression Testleri

Playwright (TypeScript) ile yazılmış tarayıcı testleri. Aynı testler hem legacy hem yeni uygulamada çalışır.

## Kurulum (bir kere)

```powershell
cd tests/e2e
npm install
npx playwright install chromium
```

## Çalıştırma

**Legacy uygulamada:** Visual Studio'da legacy uygulama çalışıyor olmalı.

```powershell
npx playwright test
```

Varsayılan adres `http://localhost:50586`. Sende port farklıysa:

```powershell
$env:BASE_URL="http://localhost:PORT"; npx playwright test
```

**Yeni uygulamada:** Aynı komut, adres yeni uygulamanınki olacak şekilde.

```powershell
$env:BASE_URL="http://localhost:YENI_PORT"; npx playwright test
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