using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class StuffPower_Armor_Heat
{
    public static readonly Dictionary<string, float> VanillaStuffPower_Armor_Heat = new Dictionary<string, float>();

    static StuffPower_Armor_Heat()
    {
    }

    public static void Initialize()
    {
        saveVanillaStuffPower_Armor_HeatValues();
        setCustomStuffPower_Armor_HeatValues();
    }

    private static void saveVanillaStuffPower_Armor_HeatValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaStuffPower_Armor_Heat[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat);
        }
    }

    private static void setCustomStuffPower_Armor_HeatValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomStuffPower_Armor_Heat == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Heat.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Heat,
                ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Heat[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom StuffPower_Armor_Heat for {counter} stufftypes.");
        }
    }

    public static void ResetStuffPower_Armor_HeatToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Heat,
                VanillaStuffPower_Armor_Heat[thingDef.defName]);
        }
    }
}