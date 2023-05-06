using HtmlAgilityPack;
using PuppeteerSharp;

namespace CitadelScraper.Services;

internal class ProductInfoPageContentService
{
    public async Task<string?> GetProductPageContentAsync(IPage page, string url)
    {
        Console.WriteLine("Beginning scraping for {0}", url);

        await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3312.0 Safari/537.36");
        await page.GoToAsync($"https://www.games-workshop.com/en-GB/{url}?format=json");
        var content = await page.GetContentAsync();

        Console.WriteLine("Fetched json content from {0}", url);

        Console.WriteLine("Beginning Deserializing json content for {0}", url);

        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        var jsonContent = doc.DocumentNode
            .Descendants("pre")
            .First()
            .InnerHtml;

        return jsonContent;
    }
}
