using CitadelScraper.Enums;

namespace CitadelScraper.Contracts.Services;

public interface IProductLinkService
{
    Task<IEnumerable<string>> FetchLinksAsync(Faction faction);
    Task<IEnumerable<string>> FetchLinksAsync(Government government);
}