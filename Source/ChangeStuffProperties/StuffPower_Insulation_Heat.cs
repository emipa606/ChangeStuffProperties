using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class StuffPower_Insulation_Heat
{
    public static readonly Dictionary<string, float> VanillaStuffPower_Insulation_Heat = new();

    static StuffPower_Insulation_Heat()
    {
    }

    public static void Initialize()
    {
        saveVanillaStuffPower_Insulation_HeatValues();
        setCustomStuffPower_Insulation_HeatValues();
    }

    private static void saveVanillaStuffPower_Insulation_HeatValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaStuffPower_Insulation_Heat[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat);
        }
    }

    private static void setCustomStuffPower_Insulation_HeatValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomStuffPower_Insulation_Heat == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Insulation_Heat.ContainsKey(
                    thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Heat,
                ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Insulation_Heat[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom StuffPower_Insulation_Heat for {counter} stufftypes.");
        }
    }

    public static void ResetStuffPower_Insulation_HeatToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Heat,
                VanillaStuffPower_Insulation_Heat[thingDef.defName]);
        }
    }
}