import { test, expect, type Page } from '@playwright/test';
import { isLegacy } from '../support/target';

// Her ürün satırında bir "Details" bağlantısı var; ürün sayısını bununla sayıyoruz.
function detailsLinks(page: Page) {
  return page.getByRole('link', { name: 'Details', exact: true });
}

test.describe('Ürün listeleme', () => {
  test('ilk sayfada 10 ürün listeleniyor', async ({ page }) => {
    await page.goto('/');

    await expect(detailsLinks(page)).toHaveCount(10);
    await expect(page.getByText(/create new/i)).toBeVisible();
    await expect(page.getByText(/Showing 10 of 12 products/)).toBeVisible();
    await expect(page.getByText(/Page 1 - 2/)).toBeVisible();
  });

  test('ilk ürünün bilgileri doğru gösteriliyor', async ({ page }) => {
    await page.goto('/');

    const row = page.getByRole('row').filter({ hasText: '.NET Bot Black Hoodie' });
    await expect(row).toContainText('.NET');
    await expect(row).toContainText('T-Shirt');
    await expect(row).toContainText('19.50');    await expect(row).toContainText('1.png');
    await expect(row).toContainText('100');
  });

  test('Next ile ikinci sayfaya geçiliyor', async ({ page }) => {
    await page.goto('/');
    await page.getByRole('link', { name: 'Next', exact: true }).click();

    await expect(page).toHaveURL(/\/Default\/index\/1\/size\/10/);
    await expect(detailsLinks(page)).toHaveCount(2);
    await expect(page.getByText('Cup<T> Sheet').first()).toBeVisible();
    await expect(page.getByText('Prism White TShirt').first()).toBeVisible();
  });

  test('Previous ile birinci sayfaya dönülüyor', async ({ page }) => {
    await page.goto('/Default/index/1/size/10');
    await page.getByRole('link', { name: 'Previous', exact: true }).click();

    await expect(detailsLinks(page)).toHaveCount(10);
    await expect(page.getByText('.NET Bot Black Hoodie').first()).toBeVisible();
  });

  test('olmayan sayfada "No data was returned." gösteriliyor', async ({ page }) => {
    await page.goto('/Default/index/99/size/10');

    await expect(page.getByText('No data was returned.')).toBeVisible();
    await expect(detailsLinks(page)).toHaveCount(0);
  });

  // ---- Bilinen legacy hataları: legacy'de başarısız olması beklenir, yeni sürümde geçmeli ----

  test('son sayfada gösterilen ürün sayısı doğru yazıyor', async ({ page }) => {
    test.fail(isLegacy, 'Bilinen legacy hatası: son sayfada her zaman "Showing 10" yazıyor');

    await page.goto('/Default/index/1/size/10');
    await expect(page.getByText(/Showing 2 of 12 products/)).toBeVisible();
  });

  test('negatif sayfa indeksi uygulamayı çökertmiyor', async ({ page }) => {
    test.fail(isLegacy, 'Bilinen legacy hatası: index -1 ArgumentException ile çöküyor');

    const response = await page.goto('/Default/index/-1/size/10');
    expect(response?.status()).toBeLessThan(500);
    await expect(page.getByText('Server Error')).toHaveCount(0);
  });
});