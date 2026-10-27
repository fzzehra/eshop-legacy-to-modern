import { expect, type Page } from '@playwright/test';

export function formField(page: Page, name: string) {
  return page
    .locator('.form-group')
    .filter({
      has: page.locator('label').getByText(name, { exact: true }),
    })
    .locator('input, select, textarea');
}

export function newTestProductName(): string {
  return `E2E-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`;
}
export async function findProductId(
  page: Page,
  productName: string
): Promise<string> {
  for (let index = 0; index < 100; index++) {
    await page.goto(`/Default/index/${index}/size/10`);

    const row = page.getByRole('row').filter({
      hasText: productName,
    });

    if (await row.count() > 0) {
      const href = await row
        .getByRole('link', { name: 'Details', exact: true })
        .getAttribute('href');

      const id = href?.match(/\/(\d+)\/?$/)?.[1];

      if (!id) {
        throw new Error(`Ürün ID'si alınamadı: ${productName}`);
      }

      return id;
    }

    // Ürün bulunmayan sayfaya geldiysek aramayı bitir.
    if (await page.getByText('No data was returned.').count() > 0) {
      break;
    }
  }

  throw new Error(`Ürün listede bulunamadı: ${productName}`);
}
export async function deleteTestProduct(
  page: Page,
  productId: string,
  productName: string
): Promise<void> {
  if (!/^E2E-[A-Za-z0-9-]+$/.test(productName)) {
    throw new Error(`Test ürünü olmayan ad silinemez: ${productName}`);
  }

  await page.goto(`/Catalog/Delete/${productId}`);

  const nameValue = page.locator('dl').locator('dd').first();

  // Silinecek ürünün adını kesin olarak doğrula.
  await expect(nameValue).toHaveText(productName);

  await page.getByRole('button', { name: /delete/i }).click();

  await expect(page).toHaveURL(/\/$/);
}