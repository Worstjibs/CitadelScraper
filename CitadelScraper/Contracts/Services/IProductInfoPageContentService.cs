using PuppeteerSharp;

namespace CitadelScraper.Contracts.Services;

public interface IProductInfoPageContentService
{
    Task<string> GetProductPageContentAsync(IPage page, string url);
}