# Legacy Uygulama (Değiştirilmeden Alınmıştır)

Bu klasördeki kod, Microsoft'un açık kaynaklı **eShopModernizing** deposundan **referans amacıyla, değiştirilmeden** alınmıştır. Proje süresince bu klasördeki kod **değiştirilmez**; modernleştirilmiş uygulama `modern/` klasöründe geliştirilir.

## Kaynak
- Depo: https://github.com/dotnet-architecture/eShopModernizing
- Klasör: `eShopLegacyWebFormsSolution`
- Commit: `63bc9ec`
- Alınma tarihi: 17.10.2026

## Lisans
MIT Lisansı · Telif hakkı © Microsoft Corporation. Lisansın tam metni için bkz. [LICENSE](LICENSE).

## Hariç tutulanlar
Derleme sırasında oluşan `bin/`, `obj/`, `packages/` ve `.vs/` klasörleri kopyalanmamıştır. Bunlar ilk derlemede otomatik olarak oluşur.

## Çalıştırma
**Gereksinimler:** Windows, Visual Studio 2022 ("ASP.NET ve web geliştirme" ve "Veri depolama ve işleme" iş yükleri)

1. `eShopLegacyWebFormsSolution/eShopLegacyWebForms.sln` dosyasını Visual Studio ile açın.
2. Çözüme sağ tıklayın → **Restore NuGet Packages**.
3. **Build → Rebuild Solution**.
4. **Ctrl+F5** ile çalıştırın (IIS Express, `http://localhost:50586/`).

**Veri kaynağı** (`src/eShopLegacyWebForms/Web.config`):
- `UseMockData = true` (varsayılan): bellekteki sahte veri
- `UseMockData = false`: LocalDB (`(localdb)\MSSQLLocalDB`). Veritabanı ilk çalıştırmada otomatik oluşturulur.

**Loglar:** Uygulama log4net ile `src/eShopLegacyWebForms/logFiles/` klasörüne log yazar (bu klasör Git'e eklenmez).

## Bilinen kurulum sorunu
İlk çalıştırmada `...\bin\roslyn\csc.exe yolunun bir parçası bulunamadı` hatası alınabilir.
**Çözüm:** Visual Studio'yu kapatıp yeniden açın → **Build → Clean Solution** → **Rebuild Solution**.
Sorun devam ederse Paket Yöneticisi Konsolu'nda:
`Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -Reinstall`

## İlgili dokümanlar

- [Ayrıntılı kurulum rehberi](../docs/setup-legacy.md)
- [Başlangıç verisi](../docs/seed-data.md)
- [Legacy uygulama davranış dokümanı](../docs/behavior.md)
- [URL ve route dokümanı](../docs/routes.md)