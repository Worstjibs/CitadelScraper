using CitadelScraper.Contracts;
using CitadelScraper.Enums;
using CitadelScraper.Extensions;
using CitadelScraper.Models;
using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Json;

namespace CitadelScraper.Services;

public class ProductLinkService : IProductLinkService
{
    public async Task<IEnumerable<string>> FetchLinksAsync(Faction faction)
    {
        var url = ConstructUrl(faction);

        return await FetchLinksInternal(url);
    }

    public async Task<IEnumerable<string>> FetchLinksAsync(Government government)
    {
        var url = ConstructUrl(government);

        return await FetchLinksInternal(url);
    }

    private async Task<IEnumerable<string>> FetchLinksInternal(string searchResultsUrl)
    {
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        using var page = await browser.NewPageAsync();

        await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3312.0 Safari/537.36");
        await page.GoToAsync(searchResultsUrl);

        var content = await page.GetContentAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        var jsonContent = doc.DocumentNode
            .Descendants("pre")
            .First()
            .InnerHtml;

        var jsonParsed = JsonSerializer.Deserialize<PageSlotViewModel>(jsonContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var contentSlot = jsonParsed!
                            .Contents.First()
                            .MainContent!.First(x => x.Type == "ContentSlotMain" && x.Contents!.Any());

        var urls = contentSlot.Contents!
                        .SelectMany(x => x.Records!.Select(r => r.Attributes!.SeoUrl!.First()))
                        .ToList();

        return urls;
    }

    private string ConstructUrl(Faction faction)
    {
        var gameType = faction.GetGameType();

        return "https://www.games-workshop.com/en-GB/searchResults" +
            $"?N={(uint)gameType}+{(uint)faction}" +
            "&view=all" +
            "&format=json";
    }

    private string ConstructUrl(Government government)
    {
        var gameType = government.GetGameType();

        var factions = government.GetFactions();
        var factionIds = factions.Cast<uint>().Select(x => x.ToString());

        var factionString = string.Join('+', factionIds);

        return "https://www.games-workshop.com/en-GB/searchResults" +
            $"?N={(uint)gameType}+{factionString}" +
            "&view=all" +
            "&format=json";
    }
}
