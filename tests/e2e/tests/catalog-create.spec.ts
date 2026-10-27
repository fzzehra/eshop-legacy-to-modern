import { test, expect, type Page } from '@playwright/test';

function formField(page: Page, name: string) {
  return page.locator('.form-group')
    .filter({
      has: page.locator('label').getByText(name, { exact: true }),
    })
    .locator('input, select, textarea');
}

test.describe('Ürün ekleme', () => {

  test('Create formundaki alanlar görüntüleniyor', async ({ page }) => {
    await page.goto('/Catalog/Create');

    for (const name of [
      'Name',
      'Description',
      'Brand',
      'Type',
      'Price',
      'Stock',
      'Restock',
      'Max stock',
    ]) {
      await expect(formField(page, name)).toBeVisible();
    }

    await expect(
      page.getByRole('button', { name: /create/i })
    ).toBeVisible();
  });

  test('ürün oluşturuluyor ve ardından temizleniyor', async ({ page }) => {
    const productName = `E2E-${Date.now()}`;
    let productId: string | undefined;

    try {
      await page.goto('/Catalog/Create');

      await formField(page, 'Name').fill(productName);
      await formField(page, 'Description').fill('Playwright Create test');
      await formField(page, 'Price').fill('12.50');
      await formField(page, 'Stock').fill('10');
      await formField(page, 'Restock').fill('2');
      await formField(page, 'Max stock').fill('50');

      await page.getByRole('button', { name: /create/i }).click();

      await expect(page).toHaveURL(/\/$/);

      // Oluşturulan ürünü listede bul
      for (let index = 0; index < 20; index++) {
        await page.goto(`/Default/index/${index}/size/10`);

        const productRow = page.locator('tr').filter({
          hasText: productName,
        });

        if (await productRow.count() > 0) {
          const detailsLink = productRow.getByRole('link', {
            name: /details/i,
          });

          const href = await detailsLink.getAttribute('href');

          productId = href?.match(/\/(\d+)\/?$/)?.[1];
          break;
        }
      }

      expect(
        productId,
        'Oluşturulan ürün listede bulunamadı'
      ).toBeTruthy();

      await page.goto(`/Catalog/Details/${productId}`);

      await expect(page.locator('body')).toContainText(productName);

    } finally {
      if (productId) {
        await page.goto(`/Catalog/Delete/${productId}`);

        // Yanlış ürünün silinmesini engelle
        await expect(page.locator('body')).toContainText(productName);

        await page.getByRole('button', {
          name: /delete/i,
        }).click();

        await expect(page).toHaveURL(/\/$/);
      }
    }
  });

});