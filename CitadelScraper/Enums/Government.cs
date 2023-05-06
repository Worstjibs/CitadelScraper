using CitadelScraper.Attributes;

namespace CitadelScraper.Enums;

public enum Government
{
    [GameType(GameType.W40K)]
    SpaceMarines,
    [GameType(GameType.W40K)]
    ArmiesOfTheImperium,
    [GameType(GameType.W40K)]
    ArmiesOfChaos,
    [GameType(GameType.W40K)]
    XenosArmies
}
