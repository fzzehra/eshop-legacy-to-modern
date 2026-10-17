using Microsoft.Playwright;

namespace Tests;

public class ModernHomePageTests
{
    [Fact]
    [Trait("Category", "ModernE2E")]
    public async Task HomePage_ShouldLoad()
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

        // Navigate to the modern app's home page.
        var baseUrl = Environment.GetEnvironmentVariable("MODERN_BASE_URL")
                      ?? "http://localhost:5232";

        var response = await page.GotoAsync(baseUrl.TrimEnd('/') + "/");

        // Verify that the page loaded successfully.
        Assert.NotNull(response);
        Assert.True(response.Ok, $"Home page returned HTTP {response.Status}.");
        Assert.Equal("Home", await page.TitleAsync());
        Assert.True(
            await page.Locator("h1").IsVisibleAsync(),
            "The home page should display a heading.");
    }
}
