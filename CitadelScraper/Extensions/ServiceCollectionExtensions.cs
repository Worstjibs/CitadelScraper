using CitadelScraper.Adapters;
using CitadelScraper.Contracts.Adapaters;
using CitadelScraper.Contracts.Services;
using CitadelScraper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CitadelScraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCitadelScraper(this IServiceCollection services)
    {
        services.AddSingleton<IProductLinkService, ProductLinkService>();
        services.AddSingleton<IProductInfoPageContentService, ProductInfoPageContentService>();
        services.AddSingleton<IPageViewModelAdapater, PageViewModelAdapater>();

        services.AddSingleton<ICitadelScraperService, CitadelScraperService>();

        return services;
    }
}
