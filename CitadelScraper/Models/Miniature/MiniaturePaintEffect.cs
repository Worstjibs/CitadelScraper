namespace CitadelScraper.Models.Miniature;

public class MiniaturePaintEffect
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public IEnumerable<MiniaturePaint> Paints { get; set; } = Enumerable.Empty<MiniaturePaint>();
}
