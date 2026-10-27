
import { test, expect } from '@playwright/test';
import {
  formField,
  newTestProductName,
  findProductId,
  deleteTestProduct,
} from '../support/catalog-helpers';

test.describe('Ürün silme', () => {
  test('oluşturulan ürün siliniyor ve artık listede görünmüyor', async ({ page }) => {
    const productName = newTestProductName();
    let productId: string | undefined;
    let deleted = false;

    try {
      // 1. Test ürünü oluştur
      await page.goto('/Catalog/Create');

      await formField(page, 'Name').fill(productName);
      await formField(page, 'Description').fill('Playwright Delete test');
      await formField(page, 'Price').fill('12.50');
      await formField(page, 'Stock').fill('10');
      await formField(page, 'Restock').fill('2');
      await formField(page, 'Max stock').fill('50');

      await page.getByRole('button', { name: /create/i }).click();
      await expect(page).toHaveURL(/\/$/);

      // 2. Oluşturulan ürünün ID'sini bul
      productId = await findProductId(page, productName);

      // 3. Ürünün gerçekten var olduğunu doğrula
      await page.goto(`/Catalog/Details/${productId}`);
      await expect(page.locator('dl dd').first()).toHaveText(productName);

      // 4. Ürünü sil
      await deleteTestProduct(page, productId, productName);
      deleted = true;

      // 5. Silinen ürün artık listede bulunmamalı
      await expect(
        findProductId(page, productName)
      ).rejects.toThrow('Ürün listede bulunamadı');

    } finally {
      // Test doğrulama aşamasında hata verirse ürünü temizlemeyi dene.
      if (productId && !deleted) {
        await deleteTestProduct(page, productId, productName);
      }
    }
  });
});
