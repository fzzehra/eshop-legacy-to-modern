using Microsoft.Playwright;

namespace Tests;

public class LegacyProductListingTests
{
    [Fact]
    [Trait("Category", "LegacyE2E")]
    public async Task ProductListing_ShouldDisplayProducts()
    {
        // Launch the Chromium browser.
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = true
            });

        // Open a new browser page.
        var page = await browser.NewPageAsync();

        // Navigate to the legacy product listing page.
        var baseUrl = Environment.GetEnvironmentVariable("LEGACY_BASE_URL")
                      ?? "http://localhost:5000";

        await page.GotoAsync(baseUrl.TrimEnd('/') + "/Default.aspx");

        // Verify that at least one product is displayed.
        var products = page.Locator("tr:has(.esh-price)");

        Assert.True(
            await products.CountAsync() > 0,
            "The legacy product listing page should display at least one product.");
    }
}