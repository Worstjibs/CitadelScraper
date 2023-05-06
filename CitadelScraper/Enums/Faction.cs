using CitadelScraper.Attributes;

namespace CitadelScraper.Enums;

public enum Faction : uint
{
    // Space Marines
    [Government(Government.SpaceMarines)]
    SpaceMarines = 3694373482,
    [Government(Government.SpaceMarines)]
    BlackTemplars = 3984380624,
    [Government(Government.SpaceMarines)]
    BloodAngels = 3984380624,
    [Government(Government.SpaceMarines)]
    DarkAngels = 1176910732,
    [Government(Government.SpaceMarines)]
    DeathWatch = 49637768,
    [Government(Government.SpaceMarines)]
    GreyKnights = 3927560896,
    [Government(Government.SpaceMarines)]
    ImperialFists = 1915851837,
    [Government(Government.SpaceMarines)]
    IronHands = 2509539302,
    [Government(Government.SpaceMarines)]
    RavenGuard = 1252943013,
    [Government(Government.SpaceMarines)]
    Salamanders = 1953378169,
    [Government(Government.SpaceMarines)]
    SpaceWolves = 2160870842,
    [Government(Government.SpaceMarines)]
    Ultramarines = 2317334297,
    [Government(Government.SpaceMarines)]
    WhiteScars = 1504000890,

    // Imperium
    [Government(Government.ArmiesOfTheImperium)]
    AdeptaSoroitas = 3639190268,
    [Government(Government.ArmiesOfTheImperium)]
    AdeptusCustodes = 2129893125,
    [Government(Government.ArmiesOfTheImperium)]
    AdeptusMechanicus = 1001313688,
    [Government(Government.ArmiesOfTheImperium)]
    AstraMilitarum = 1138996955,
    [Government(Government.ArmiesOfTheImperium)]
    ImperialKnights = 725436515,
    [Government(Government.ArmiesOfTheImperium)]
    Inquisition = 2964486909,
    [Government(Government.ArmiesOfTheImperium)]
    OfficioAssassinorum = 1464247808
}
