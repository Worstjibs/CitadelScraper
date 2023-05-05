using _40kScraper.Console;
using _40kScraper.Console.Models;

namespace _40kScraper;

public class MiniatureDetails
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public ProductImage[] Images = Array.Empty<ProductImage>();
    public PaintEffect[] PaintEffects { get; set; } = Array.Empty<PaintEffect>();

    public override string? ToString()
    {
        return $"{Name}, Url: {Url}, Images: {string.Join(',', Images.Select(x => x.Url))}";
    }
}