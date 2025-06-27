using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class MarketValue
{
    public static readonly Dictionary<string, float> VanillaMarketValues = new();

    static MarketValue()
    {
    }

    public static void Initialize()
    {
        saveVanillaMarketValueValues();
        setCustomMarketValueValues();
    }

    private static void saveVanillaMarketValueValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaMarketValues[thingDef.defName] = thingDef.BaseMarketValue;
        }
    }

    private static void setCustomMarketValueValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomMarketValue == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomMarketValue.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.MarketValue,
                ChangeStuffProperties_Mod.instance.Settings.CustomMarketValue[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom MarketValue for {counter} stufftypes.");
        }
    }

    public static void ResetMarketValueToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.MarketValue, VanillaMarketValues[thingDef.defName]);
        }
    }
}