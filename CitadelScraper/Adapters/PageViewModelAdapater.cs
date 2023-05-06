using CitadelScraper.Models;
using System.Text.Json;

namespace CitadelScraper.Adapters;

public class PageViewModelAdapater
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

            miniatureDetails.PaintMethods = paintGuides!.Methods;
        }

        return miniatureDetails;
    }
}
