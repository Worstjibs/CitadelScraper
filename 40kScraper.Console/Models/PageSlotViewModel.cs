namespace _40kScraper.Console.Models;

public class PageSlotViewModel
{
    public int RuleLimit { get; set; }

    public PageDetailViewModel[] Contents { get; set; } = Array.Empty<PageDetailViewModel>();
}
