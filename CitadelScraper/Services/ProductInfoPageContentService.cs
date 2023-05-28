using CitadelScraper.Contracts.Services;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace CitadelScraper.Services;

internal class ProductInfoPageContentService : IProductInfoPageContentService
{
    public async Task<string> GetProductPageContentAsync(IPage page, string url)
    {
        Console.WriteLine("Beginning scraping for {0}", url);

        await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3312.0 Safari/537.36");
        await page.GoToAsync($"https://www.games-workshop.com/en-GB/{url}?format=json");
        var content = await page.GetContentAsync();

        Console.WriteLine("Fetched json content from {0}", url);

        Console.WriteLine("Beginning Deserializing json content for {0}", url);

        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        var preElement = doc.DocumentNode
            .Descendants("pre")
            .FirstOrDefault();

        if (preElement is null)
            throw new HtmlWebException("No pre element on page");

        var jsonContent = preElement.InnerHtml;

        if (jsonContent is null)
            throw new HtmlWebException("pre element contains no content");

        return jsonContent;
    }
}
