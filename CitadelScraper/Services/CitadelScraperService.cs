﻿using CitadelScraper.Contracts.Adapaters;
using CitadelScraper.Contracts.Services;
using CitadelScraper.Enums;
using CitadelScraper.Models.Miniature;
using CitadelScraper.Models.PageViewModel;
using PuppeteerSharp;
using System.Text.Json;

namespace CitadelScraper.Services;

public class CitadelScraperService : ICitadelScraperService
{
    private const int _partitions = 10;

    private readonly IProductLinkService _linkService;
    private readonly IProductInfoPageContentService _productInfoPageContentService;
    private readonly IPageViewModelAdapater _pageViewModelAdapater;

    public CitadelScraperService(
        IProductLinkService linkService,
        IProductInfoPageContentService productInfoPageContentService,
        IPageViewModelAdapater pageViewModelAdapater)
    {
        _linkService = linkService;
        _productInfoPageContentService = productInfoPageContentService;
        _pageViewModelAdapater = pageViewModelAdapater;
    }

    public async Task<IEnumerable<MiniatureDetails>> GetMiniaturesAsync(Faction faction)
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var links = await _linkService.FetchLinksAsync(faction);

        var batches = PartitionUrls(links, _partitions);

        var processTasks = batches.Select(ProcessBatchAsync);

        var processedBatches = await Task.WhenAll(processTasks);
        var processedMiniatures = processedBatches.SelectMany(x => x).ToList();

        return processedMiniatures;
    }

    public async Task<IEnumerable<MiniatureDetails>> GetMiniaturesAsync(Government government)
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var links = await _linkService.FetchLinksAsync(government);

        var batches = PartitionUrls(links, _partitions);

        var processTasks = batches.Select(ProcessBatchAsync);

        var processedBatches = await Task.WhenAll(processTasks);
        var processedMiniatures = processedBatches.SelectMany(x => x).ToList();

        return processedMiniatures;
    }

    private async Task<IEnumerable<MiniatureDetails>> ProcessBatchAsync(IEnumerable<string> urlBatch, int batchIndex)
    {
        Console.WriteLine($"Beginning batch {batchIndex}");

        var detailsForBatch = new List<MiniatureDetails>();

        var browser = await ResetBrowserAsync();
        var page = (await browser.PagesAsync()).First();

        foreach (var item in urlBatch)
        {
            detailsForBatch.Add(await RetryAsync(browser, page, item));
        }

        browser.Dispose();

        return detailsForBatch;
    }

    private async Task<MiniatureDetails> RetryAsync(IBrowser browser, IPage page, string url)
    {
        var retries = 10;

        for (var i = 0; i < retries; i++)
        {
            try
            {
                var jsonContent = await _productInfoPageContentService.GetProductPageContentAsync(page, url);

                var jsonParsed = JsonSerializer.Deserialize<PageSlotViewModel>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                Console.WriteLine("Finished Deserializing json content for {0}", url);

                Console.WriteLine("Beginning mapping to MiniatureDetails for {0}", url);

                var details = _pageViewModelAdapater.ToMiniatureDetails(jsonParsed!);

                Console.WriteLine("Finished mapping to MiniatureDetails {0}", url);

                return details;
            } catch (NullReferenceException)
            {
                throw;
            } catch (JsonException)
            {
                throw;
            } catch (Exception)
            {
                Console.WriteLine("Error occured whilst scraping page.");
                Console.WriteLine("Resetting browser for url {0}", url);

                // Something went wrong with the batch, reset the browser
                browser.Dispose();
                browser = await ResetBrowserAsync();
                page = (await browser.PagesAsync()).First();
            }
        }

        throw new Exception($"Retries exhausted for url {url}");
    }

    private async Task<IBrowser> ResetBrowserAsync()
    {
        return await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
    }

    private IEnumerable<IEnumerable<string>> PartitionUrls(IEnumerable<string> urls, int numberOfPartitions)
    {
        var partitions = urls
                .Select((x, i) => new
                {
                    Partition = i % numberOfPartitions,
                    Url = x
                })
                .GroupBy(p => p.Partition, g => g.Url)
                .Select(x => x.ToArray())
                .ToArray();

        return partitions;
    }
}
