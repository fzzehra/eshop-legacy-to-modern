import { test, expect } from '@playwright/test';
import {
  formField,
  newTestProductName,
  findProductId,
  deleteTestProduct,
} from '../support/catalog-helpers';

test.describe('Ürün düzenleme', () => {
  test('oluşturulan ürün düzenleniyor ve değişiklikler kaydediliyor', async ({ page }) => {
    const originalName = newTestProductName();
    const updatedName = `${originalName}-EDIT`;
    let productId: string | undefined;
    let cleanupName = originalName;

    try {
      // 1. Test ürününü oluştur
      await page.goto('/Catalog/Create');

      await formField(page, 'Name').fill(originalName);
      await formField(page, 'Description').fill('Before edit');
      await formField(page, 'Price').fill('12.50');
      await formField(page, 'Stock').fill('10');
      await formField(page, 'Restock').fill('2');
      await formField(page, 'Max stock').fill('50');

      await page.getByRole('button', { name: /create/i }).click();
      await expect(page).toHaveURL(/\/$/);

      productId = await findProductId(page, originalName);

      // 2. Ürünü düzenle
      await page.goto(`/Catalog/Edit/${productId}`);

      await expect(formField(page, 'Name')).toHaveValue(originalName);

      await formField(page, 'Name').fill(updatedName);
      await formField(page, 'Description').fill('After edit');
      await formField(page, 'Price').fill('25.50');

      await page.getByRole('button', { name: /save/i }).click();
      cleanupName = updatedName;

      // 3. Kaydedilen bilgileri doğrula
      await page.goto(`/Catalog/Edit/${productId}`);

      await expect(formField(page, 'Name')).toHaveValue(updatedName);
      await expect(formField(page, 'Description')).toHaveValue('After edit');
      await expect(formField(page, 'Price')).toHaveValue('25.50');

    } finally {
      // 4. Yalnızca test ürününü temizle
      if (productId) {
        await deleteTestProduct(page, productId, cleanupName);
      }
    }
  });
});