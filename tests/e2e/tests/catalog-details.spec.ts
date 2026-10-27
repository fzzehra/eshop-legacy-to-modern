import { test, expect } from '@playwright/test';
import { isLegacy } from '../support/target';

test.describe('Ürün detayı', () => {
  test('ürünün bütün bilgileri gösteriliyor', async ({ page }) => {
    await page.goto('/Catalog/Details/1');

    const body = page.locator('body');

    // Alan adları
    for (const label of ['Name', 'Description', 'Brand', 'Type', 'Price',
                         'Picture name', 'Stock', 'Restock', 'Max stock']) {
      await expect(body).toContainText(new RegExp(label, 'i'));
    }

    // Değerler
    await expect(body).toContainText('.NET Bot Black Hoodie');
    await expect(body).toContainText('T-Shirt');
    await expect(body).toContainText('19.50');
    await expect(body).toContainText('1.png');
    await expect(body).toContainText('100');
  });

  test('Edit bağlantısı düzenleme sayfasını açıyor', async ({ page }) => {
    await page.goto('/Catalog/Details/1');
    await page.getByRole('link', { name: /^edit$/i }).click();

    await expect(page).toHaveURL(/\/Catalog\/Edit\/1/);
  });

  test('Back to list bağlantısı listeye dönüyor', async ({ page }) => {
    await page.goto('/Catalog/Details/1');
    await page.getByRole('link', { name: /back to list/i }).click();

    await expect(page.getByRole('link', { name: 'Details', exact: true })).toHaveCount(10);
  });

  // ---- Bilinen legacy hataları: legacy'de başarısız olması beklenir, yeni sürümde geçmeli ----

  test('olmayan ürün için 404 dönüyor ve uygulama çökmüyor', async ({ page }) => {
    test.fail(isLegacy, 'Bilinen legacy hatası: /Catalog/Details/999 NullReferenceException ile çöküyor');

    const response = await page.goto('/Catalog/Details/999');
    expect(response?.status()).toBe(404);
    await expect(page.getByText('Server Error')).toHaveCount(0);
  });

  test('geçersiz id için 404 dönüyor ve uygulama çökmüyor', async ({ page }) => {
    test.fail(isLegacy, 'Bilinen legacy hatası: /Catalog/Details/abc FormatException ile çöküyor');

    const response = await page.goto('/Catalog/Details/abc');
    expect(response?.status()).toBe(404);
    await expect(page.getByText('Server Error')).toHaveCount(0);
  });
});