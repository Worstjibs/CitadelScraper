﻿using CitadelScraper.Contracts.Adapaters;
using CitadelScraper.Enums;
using CitadelScraper.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

var services = new ServiceCollection();
services.AddCitadelScraper();

var provider = services.BuildServiceProvider();

var scraper = provider.GetRequiredService<ICitadelScraperService>();

var startTime = Stopwatch.GetTimestamp();

var details = await scraper.GetMiniaturesAsync(Faction.Ultramarines);

Console.WriteLine($"Scraping finished. Scraped {details.Count()} records in {Stopwatch.GetElapsedTime(startTime)}");

var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonStringEnumConverter());

await File.WriteAllTextAsync("miniature_details.json", JsonSerializer.Serialize(details, jsonOptions));