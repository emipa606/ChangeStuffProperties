using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class StuffPower_Armor_Sharp
{
    public static readonly Dictionary<string, float> VanillaStuffPower_Armor_Sharp = new();

    static StuffPower_Armor_Sharp()
    {
    }

    public static void Initialize()
    {
        saveVanillaStuffPower_Armor_SharpValues();
        setCustomStuffPower_Armor_SharpValues();
    }

    private static void saveVanillaStuffPower_Armor_SharpValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaStuffPower_Armor_Sharp[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp);
        }
    }

    private static void setCustomStuffPower_Armor_SharpValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomStuffPower_Armor_Sharp == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Sharp.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Sharp,
                ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Sharp[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom StuffPower_Armor_Sharp for {counter} stufftypes.");
        }
    }

    public static void ResetStuffPower_Armor_SharpToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Sharp,
                VanillaStuffPower_Armor_Sharp[thingDef.defName]);
        }
    }
}