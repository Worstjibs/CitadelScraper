using System.Text.Json.Serialization;

namespace CitadelScraper.Models;

public class PageDetailViewModel
{
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("@type")]
    public string Type { get; set; } = string.Empty;

    public PageDetailViewModel[]? MainContent { get; set; }
    public PageDetailViewModel[]? SecondaryContent { get; set; }
    public PageDetailViewModel[]? Contents { get; set; }

    public Record? Record { get; set; }
    public Record[]? Records { get; set; }

    public string? ProductInfoDescription { get; set; }

    public ProductImage[]? ProductImages { get; set; } = Array.Empty<ProductImage>();
}
