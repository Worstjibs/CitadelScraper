using CitadelScraper.Enums;

namespace CitadelScraper.Attributes;

public class GovernmentAttribute : Attribute
{
    public GovernmentAttribute(Government government)
    {
        Government = government;
    }

    public Government Government { get; init; }
}
