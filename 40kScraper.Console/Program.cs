using CsvHelper;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Globalization;

var url = "URL HERE";

await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = true
});

var page = await browser.NewPageAsync();

await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");

await page.GoToAsync(url);

await page.WaitForSelectorAsync("h1");

var content = await page.GetContentAsync();

var doc = new HtmlDocument();
doc.LoadHtml(content);

var docImages = doc.DocumentNode
    .Descendants("img")
    .Where(x => x.HasClass("media-image"))
    .SelectMany(x => x.Attributes
        .Where(a => a.Name == "data-lazy")
        .Select(a => new { Image = a.Value }))
    .ToList();

using (var writer = new StreamWriter("ork-images.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(docImages);
}
