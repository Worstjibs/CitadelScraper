using System.Text.Json.Serialization;

namespace _40kScraper.Console.Models;

public class PageDetailViewModel
{
    [JsonPropertyName("@type")]
    public string Type { get; set; } = string.Empty;

    public PageDetailViewModel[]? MainContent { get; set; }
    public PageDetailViewModel[]? SecondaryContent { get; set; }

    public Record? Record { get; set; }

    public string? ProductInfoDescription { get; set; }

    public ProductImage[]? ProductImages { get; set; } = Array.Empty<ProductImage>();
}
