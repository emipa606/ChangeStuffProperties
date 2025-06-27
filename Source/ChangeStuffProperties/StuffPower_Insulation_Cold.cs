using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class StuffPower_Insulation_Cold
{
    public static readonly Dictionary<string, float> VanillaStuffPower_Insulation_Cold = new();

    static StuffPower_Insulation_Cold()
    {
    }

    public static void Initialize()
    {
        saveVanillaStuffPower_Insulation_ColdValues();
        setCustomStuffPower_Insulation_ColdValues();
    }

    private static void saveVanillaStuffPower_Insulation_ColdValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaStuffPower_Insulation_Cold[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold);
        }
    }

    private static void setCustomStuffPower_Insulation_ColdValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomStuffPower_Insulation_Cold == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Insulation_Cold.ContainsKey(
                    thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Cold,
                ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Insulation_Cold[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom StuffPower_Insulation_Cold for {counter} stufftypes.");
        }
    }

    public static void ResetStuffPower_Insulation_ColdToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Cold,
                VanillaStuffPower_Insulation_Cold[thingDef.defName]);
        }
    }
}