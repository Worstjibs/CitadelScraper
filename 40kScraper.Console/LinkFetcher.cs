using PuppeteerSharp;

namespace _40kScraper.Console;

public static class LinkFetcher
{
    private const string _fileName = "links.txt";

    public static async Task<IEnumerable<string>> FetchLinksAsync()
    {
        if (File.Exists(_fileName))
        {
            var urls = await File.ReadAllLinesAsync(_fileName);

            if (urls.Any())
                return urls;
        }

        return await FetchLinksInternal();
    }

    private static async Task<IEnumerable<string>> FetchLinksInternal()
    {
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        using var page = await browser.NewPageAsync();

        await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");

        await page.GoToAsync("https://www.games-workshop.com/en-GB/Warhammer-40-000?N=1125463923+3694373482+1320453649+3984380624+1176910732+49637768+1504000890+2317334297+2160870842+3927560896+1915851837+2509539302+1252943013+1953378169&Nr=AND(sku.siteId%3AGB_gw%2Cproduct.locale%3Aen_GB_gw)&Nrs=collection()%2Frecord[product.startDate+%3C%3D+1679935260000+and+product.endDate+%3E%3D+1679935260000]&view=all");

        var productGrid = await page.QuerySelectorAsync(".product-grid");

        var links = await productGrid.QuerySelectorAllAsync("a.product-item__name");
        var urlHandlers = await Task.WhenAll(links.Select(x => x.GetPropertyAsync("href")));
        var urls = (await Task.WhenAll(urlHandlers.Select(x => x.JsonValueAsync()))).Cast<string>().ToList();

        await File.AppendAllLinesAsync(_fileName, urls.Cast<string>());

        return urls;
    }
}
