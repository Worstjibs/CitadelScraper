using CitadelScraper.Models.Miniature;
using CitadelScraper.Models.PageViewModel;

namespace CitadelScraper.Contracts.Services;

public interface IPageViewModelAdapater
{
    MiniatureDetails ToMiniatureDetails(PageSlotViewModel viewModel);
}