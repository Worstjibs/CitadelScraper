using System.Text.Json.Serialization;

namespace CitadelScraper.Models.Miniature;

public class MiniaturePaint
{
    public string ProductId { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;

    public MiniaturePaintLevel? Level { get; set; }
}