using System.Text.Json.Serialization;

namespace CitadelScraper.Models.PageViewModel.Paints;

public class PaintEffect
{
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("swatchImage")]
    public string ImageUrl { get; set; } = string.Empty;
    public string UrlParam { get; set; } = string.Empty;
    public IEnumerable<Paint> Paints { get; set; } = Enumerable.Empty<Paint>();
}
