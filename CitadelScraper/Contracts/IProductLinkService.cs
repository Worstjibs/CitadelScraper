using CitadelScraper.Enums;

namespace CitadelScraper.Contracts;

public interface IProductLinkService
{
    Task<IEnumerable<string>> FetchLinksAsync(Faction faction);
}