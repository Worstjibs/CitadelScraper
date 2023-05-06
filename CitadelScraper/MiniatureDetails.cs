﻿using CitadelScraper.Models;

namespace CitadelScraper;

public class MiniatureDetails
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public ProductImage[] Images = Array.Empty<ProductImage>();
    public PaintMethods? PaintMethods { get; set; }

    public override string? ToString()
    {
        return $"{Name}, Url: {Url}, Images: {string.Join(',', Images.Select(x => x.Url))}";
    }
}