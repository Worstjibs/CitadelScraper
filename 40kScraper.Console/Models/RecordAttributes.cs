using System.Text.Json.Serialization;

namespace _40kScraper.Console.Models;

public class RecordAttributes
{
    [JsonPropertyName("product.title")]
    public string[]? Title { get; set; }

    [JsonPropertyName("product.shareUrl")]
    public string[]? ShareUrl { get; set; }
}