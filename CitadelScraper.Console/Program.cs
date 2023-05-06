using CitadelScraper.Enums;
using CitadelScraper.Services;
using System.Diagnostics;

var service = new MiniatureDetailsService(new ProductLinkService());

var startTime = Stopwatch.GetTimestamp();

var details = await service.GetMiniaturesAsync(Government.ArmiesOfTheImperium);

Console.WriteLine($"Scraping finished. Scraped {details.Count()} records in {Stopwatch.GetElapsedTime(startTime)}");