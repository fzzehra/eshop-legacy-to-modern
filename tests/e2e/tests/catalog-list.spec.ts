import { test, expect } from '@playwright/test';

test('ana sayfa açılıyor ve ürünler listeleniyor', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByText(/create new/i)).toBeVisible();
  await expect(page.getByText('.NET Bot Black Hoodie').first()).toBeVisible();
});