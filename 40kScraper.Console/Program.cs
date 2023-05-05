using _40kScraper;
using _40kScraper.Console;
using _40kScraper.Console.Adapters;
using _40kScraper.Console.Models;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Diagnostics;
using System.Text.Json;

const int take = 100;

await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

var links = await LinkFetcher.FetchLinksAsync();

var startTime = Stopwatch.GetTimestamp();

var details = new List<MiniatureDetails>();

var batches = links.Take(take).Chunk(20);
var processTasks = batches.Select(ProcessBatchAsync);

var processedBatches = await Task.WhenAll(processTasks);
var processedMiniatures = processedBatches.SelectMany(x => x).ToList();

details.AddRange(processedMiniatures);

var json = JsonSerializer.Serialize(details);
await File.WriteAllTextAsync("page_details.json", json);

var finishedTime = Stopwatch.GetElapsedTime(startTime);

Console.WriteLine($"Scraping finiehd. Scraped {take} records in {finishedTime}");


static async Task<IEnumerable<MiniatureDetails>> ProcessBatchAsync(IEnumerable<string> urlBatch, int batchIndex)
{
    Console.WriteLine($"Beginning batch {batchIndex}");

    var detailsForBatch = new List<MiniatureDetails>();

    var browser = await ResetBrowserAsync();
    var page = (await browser.PagesAsync()).First();

    foreach (var item in urlBatch)
    {
        try
        {
            Console.WriteLine("Beginning scraping for {0}", item);

            await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3312.0 Safari/537.36");
            await page.GoToAsync(item + "?format=json");

            var content = await page.GetContentAsync();

            Console.WriteLine("Fetched json content from {0}", item);


            Console.WriteLine("Beginning Deserializing json content for {0}", item);

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

            Console.WriteLine("Finished Deserializing json content for {0}", item);

            Console.WriteLine("Beginning mapping to MiniatureDetails for {0}", item);

            var details = new PageViewModelAdapater().ToMiniatureDetails(jsonParsed!);

            Console.WriteLine("Finished mapping to MiniatureDetails {0}", item);

            detailsForBatch.Add(details);
        } catch (Exception e)
        {
            // Something went wrong with the batch, reset the browser
            browser.Dispose();
            browser = await ResetBrowserAsync();
            page = (await browser.PagesAsync()).First();
        }
    }

    browser.Dispose();

    return detailsForBatch;
}

static async Task<IEnumerable<PaintEffect>> ProcessPaintEffects(IPage page, IEnumerable<HtmlNode> paintEffectLineItems)
{
    foreach (var lineItem in paintEffectLineItems)
    {
        await page.ClickAsync(".effect");

        await page.WaitForSelectorAsync("#paint-effect-detail", new WaitForSelectorOptions
        {
            Timeout = 10000
        });

        var paintEffects = paintEffectLineItems
            .Select(x => new PaintEffect
            {
                Name = x.Descendants("span").Where(x => x.HasClass("effect-name_text")).First().InnerHtml.Trim(),
                ImageUrl = x.Descendants("img").Where(x => x.HasClass("effect-image")).First().GetAttributeValue("src", string.Empty)
            })
            .ToList();
    }

    return Enumerable.Empty<PaintEffect>();
}

static async Task<IBrowser> ResetBrowserAsync()
{
    return await Puppeteer.LaunchAsync(new LaunchOptions
    {
        Headless = true,
        DefaultViewport = new ViewPortOptions
        {
            Width = 1200,
            Height = 1000,
            IsMobile = false
        }
    });
}