using CitadelScraper.Enums;

namespace CitadelScraper.Attributes;

public class GameTypeAttribute : Attribute
{
    public GameTypeAttribute(GameType gameType)
    {
        GameType = gameType;
    }

    public GameType GameType { get; init; }
}
