using System.Text.Json.Serialization;

namespace CitadelScraper.Models.PageViewModel;

public class RecordAttributes
{
    [JsonPropertyName("product.title")]
    public string[]? ProductTitle { get; set; }

    [JsonPropertyName("product.shareUrl")]
    public string[]? ShareUrl { get; set; }

    [JsonPropertyName("product.seoUrl")]
    public string[]? SeoUrl { get; set; }

    [JsonPropertyName("product.colours")]
    public ProductColours[]? ProductColours { get; set; }

    [JsonPropertyName("product.paintingGuide")]
    public string[]? PaintingGuide { get; set; }
}
