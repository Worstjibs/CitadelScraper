using CitadelScraper.Models.PageViewModel;

namespace CitadelScraper.Models.Miniature;

public class MiniatureDetails
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public MiniatureProductImage[] Images = Array.Empty<MiniatureProductImage>();
    public MiniaturePaintMethods? PaintMethods { get; set; }

    public override string? ToString()
    {
        return $"{Name}, Url: {Url}, Images: {string.Join(',', Images.Select(x => x.Url))}";
    }
}