using _40kScraper.Console.Models;

namespace _40kScraper.Console.Adapters;

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


        return new MiniatureDetails
        {
            Name = productDetail.Record!.Attributes!.Title!.First(),
            Url = productDetail.Record!.Attributes!.ShareUrl!.First(),
            Description = productInfo.ProductInfoDescription!,
            Images = productImages.ProductImages!
        };
    }
}
