public class MiniatureDetails
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string[] ImageUrls { get; set; }

    public override string? ToString()
    {
        return $"{Name}, Url: {Url}, Images: {string.Join(',', ImageUrls)}";
    }
}