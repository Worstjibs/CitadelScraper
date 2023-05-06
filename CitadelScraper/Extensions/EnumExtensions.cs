using CitadelScraper.Attributes;
using CitadelScraper.Enums;
using System.Reflection;

namespace CitadelScraper.Extensions;

public static class EnumExtensions
{
    public static Government GetGovernment(this Faction faction)
    {
        var governmentAttribute = faction.GetCustomAttribute<GovernmentAttribute>();

        return governmentAttribute.Government;
    }
    public static GameType GetGameType(this Faction faction)
    {
        var government = GetGovernment(faction);

        var gameTypeAttribute = government.GetCustomAttribute<GameTypeAttribute>();

        return gameTypeAttribute.GameType;
    }

    private static TAttr GetCustomAttribute<TAttr>(this Enum enumVal)
        where TAttr : Attribute
    {
        var enumType = enumVal.GetType();

        var memberInfo = enumType.GetMember(enumVal.ToString()).FirstOrDefault();
        if (memberInfo is null)
            throw new ArgumentException($"Invalid value provided for Enum of type ${enumType}: {enumVal}", nameof(enumVal));

        var attribute = memberInfo.GetCustomAttribute<TAttr>();
        if (attribute is null)
            throw new ArgumentException($"Attribute {nameof(TAttr)} does not exist on enum member {enumVal}");

        return attribute;
    }
}
