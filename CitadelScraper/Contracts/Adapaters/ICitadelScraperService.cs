using CitadelScraper.Enums;
using CitadelScraper.Models.Miniature;

namespace CitadelScraper.Contracts.Adapaters;

public interface ICitadelScraperService
{
    Task<IEnumerable<MiniatureDetails>> GetMiniaturesAsync(Faction faction);
    Task<IEnumerable<MiniatureDetails>> GetMiniaturesAsync(Government government);
}