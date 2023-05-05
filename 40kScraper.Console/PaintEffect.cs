namespace _40kScraper.Console;

public class PaintEffect
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public IEnumerable<Paint> Paints { get; set; } = Enumerable.Empty<Paint>();
}

public class Paint
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}