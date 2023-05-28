using CitadelScraper.Contracts.Services;
using CitadelScraper.Models.Miniature;
using CitadelScraper.Models.PageViewModel;
using CitadelScraper.Models.PageViewModel.Paints;
using System.Diagnostics;
using System.Text.Json;

namespace CitadelScraper.Adapters;

public class PageViewModelAdapater : IPageViewModelAdapater
{
    public MiniatureDetails ToMiniatureDetails(PageSlotViewModel viewModel)
    {
        var twoColumnProductDetails = viewModel
                                        .Contents
                                        .First()
                                        .MainContent!.First(x
                                            => x.Type == "TwoColumnProductDetails");

        var productDetail = twoColumnProductDetails.SecondaryContent!.First(x => x.Type == "ProductDetail");
        var productImages = twoColumnProductDetails.MainContent!.First(x => x.Type == "ProductMedia");
        var productInfo = viewModel.Contents.First().MainContent!.First(x => x.Type == "ProductInfo");

        var miniatureDetails = new MiniatureDetails
        {
            Name = productDetail.Record!.Attributes!.ProductTitle!.First(),
            Url = productDetail.Record!.Attributes!.ShareUrl!.First(),
            Description = productInfo.ProductInfoDescription!,
            Images = productImages.ProductImages!
                .Select(x => new MiniatureProductImage
                {
                    Name = x.Name,
                    Url = x.Url
                }).ToArray()
        };

        var paintpageContent = viewModel.Contents.First().MainContent!.FirstOrDefault(x => x.Type == "PaintPageContentSlotMain");
        if (paintpageContent is not null)
        {
            var paintEffectAttributes = paintpageContent
                            .Contents!.First()
                            .MainContent!.First(x => x.Type == "PaintEffects")
                            .Record!
                            .Attributes!;

            var paintGuideString = paintEffectAttributes.PaintingGuide!.First();
            var paintGuides = JsonSerializer.Deserialize<PaintGuides>(paintGuideString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var paintMethods = paintGuides!.Methods;

            if (paintMethods is not null)
                miniatureDetails.PaintMethods = MapPaintMethods(paintMethods);
        }

        return miniatureDetails;
    }

    private MiniaturePaintMethods MapPaintMethods(PaintMethods paintMethods)
    {
        var miniaturePaintMethods = new MiniaturePaintMethods
        {
            Classic = paintMethods.Classic?.Select(MapPaintEffect).ToArray(),
            Contrast = paintMethods.Contrast?.Select(MapPaintEffect).ToArray()
        };

        return miniaturePaintMethods;
    }

    private MiniaturePaintEffect MapPaintEffect(PaintEffect paintEffect)
    {
        var miniaturePaintEffect = new MiniaturePaintEffect
        {
            Name = paintEffect.Name,
            ImageUrl = paintEffect.ImageUrl
        };

        if (!paintEffect.Paints.Any())
            return miniaturePaintEffect;

        Action<Dictionary<string, MiniaturePaintLevel>, string[], MiniaturePaintLevel> insertIntoDictionary = (dict, productIds, level) =>
        {
            foreach (var productId in productIds)
            {
                dict[productId] = level;
            }
        };

        var levelDict = new Dictionary<string, MiniaturePaintLevel>();

        var splitPaints = paintEffect.UrlParam.Split('-');

        if (splitPaints.Length == 2)
        {
            var battleReady = splitPaints[0].Split(',');
            var paradeReady = splitPaints[1].Split(',');

            insertIntoDictionary(levelDict, battleReady, MiniaturePaintLevel.BattleReady);
            insertIntoDictionary(levelDict, paradeReady, MiniaturePaintLevel.ParadeReady);

            if (battleReady.Any(string.IsNullOrWhiteSpace) || paradeReady.Any(string.IsNullOrWhiteSpace))
                Console.WriteLine("Issue here!");
        }

        miniaturePaintEffect.Paints = paintEffect.Paints
            .Where(x => x.ProductId is not null && x.ImageName is not null)
            .Select(x => new MiniaturePaint
            {
                ImageName = x.ImageName!,
                ProductId = x.ProductId!,
                Level = levelDict[x.ProductId!]
            }).ToList();

        return miniaturePaintEffect;
    }
}
